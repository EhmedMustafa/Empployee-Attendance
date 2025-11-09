const dotenv = require('dotenv');
const path = require('path');
dotenv.config({ path: path.resolve(__dirname, '.env') });

const express = require('express');
const cors = require('cors');
const sql = require('mssql');
const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');

const app = express();
app.use(cors());
app.use(express.json());

// Request logging middleware
app.use((req, res, next) => {
    console.log(`${req.method} ${req.url}`);
    console.log('Headers:', req.headers);
    console.log('Body:', req.body);
    next();
});

// MSSQL connection configuration
const {
    DB_HOST = 'localhost',
    DB_USER = 'Ehmed',
    DB_PASSWORD = '@zercell',
    DB_NAME = 'EmployeeTable'
} = process.env;

console.log('MSSQL config:', {
    server: DB_HOST,
    user: DB_USER,
    database: DB_NAME
});

const sqlConfig = {
    user: DB_USER,
    password: DB_PASSWORD,
    database: DB_NAME,
    server: DB_HOST,
    port: 1433,
    options: {
        encrypt: false,
        trustServerCertificate: true
    }
};

console.log('SQL Config:', sqlConfig);

let pool;
sql.connect(sqlConfig).then(p => {
    pool = p;
    console.log('Connected to MSSQL');
}).catch(err => {
    console.error('MSSQL connection error:', err);
});

// Authentication Middleware
const authenticateToken = (req, res, next) => {
    const authHeader = req.headers['authorization'];
    const token = authHeader && authHeader.split(' ')[1];

    if (!token) {
        return res.status(401).json({ message: 'Authentication required' });
    }

    jwt.verify(token, process.env.JWT_SECRET || 'your_jwt_secret', (err, user) => {
        if (err) {
            return res.status(403).json({ message: 'Invalid token' });
        }
        req.user = user;
        next();
    });
};

// Login endpoint
app.post('/api/login', async (req, res) => {
    try {
        const { username, password } = req.body;
        console.log('Login attempt:', { username });
        const result = await pool.request()
            .input('UserName', sql.VarChar, username)
            .query('SELECT * FROM employees WHERE UserName = @UserName');
        console.log('DB query result:', result);

        const rows = result.recordset;
        if (rows.length === 0) {
            return res.status(401).json({ message: 'Invalid credentials' });
        }

        const employee = rows[0];
        const validPassword = await bcrypt.compare(password, employee.PassWordHash);

        if (!validPassword) {
            return res.status(401).json({ message: 'Invalid credentials' });
        }

        const token = jwt.sign(
            { id: employee.EmployeeId, isAdmin: employee.IsAdmin },
            process.env.JWT_SECRET || 'your_jwt_secret',
            { expiresIn: '1d' }
        );

        res.json({
            token,
            employee: {
                id: employee.EmployeeId,
                fullName: employee.FullName,
                isAdmin: employee.IsAdmin
            }
        });
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
});

// Attendance endpoints
app.post('/api/attendance/checkin', authenticateToken, async (req, res) => {
    try {
        const { employeeId, storeId } = req.body;
        const result = await pool.request()
            .input('EmployeeId', sql.Int, employeeId)
            .input('StoreId', sql.Int, storeId)
            .query('INSERT INTO attendance (EmployeeId, StoreId, Date, CheckIn) VALUES (@EmployeeId, @StoreId, CAST(GETDATE() AS DATE), GETDATE()); SELECT SCOPE_IDENTITY() AS insertId;');
        res.json({ message: 'Check-in successful', id: result.recordset[0].insertId });
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
});

app.post('/api/attendance/checkout/:id', authenticateToken, async (req, res) => {
    try {
        const { id } = req.params;
        await pool.request()
            .input('AttendanceId', sql.Int, id)
            .query('UPDATE attendance SET CheckOut = GETDATE() WHERE AttendanceId = @AttendanceId');
        res.json({ message: 'Check-out successful' });
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
});

// Get attendance history
app.get('/api/attendance/history/:employeeId', authenticateToken, async (req, res) => {
    try {
        const { employeeId } = req.params;
        const result = await pool.request()
            .input('EmployeeId', sql.Int, employeeId)
            .query(`SELECT a.*, s.StoreName 
                    FROM attendance a 
                    JOIN stores s ON a.StoreId = s.StoreId 
                    WHERE a.EmployeeId = @EmployeeId 
                    ORDER BY a.Date DESC`);
        res.json(result.recordset);
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
});

// Public: list stores (no auth) - convenient for clients that just need store list
app.get('/api/stores', async (req, res) => {
    try {
        console.log('Trying to execute stores query...');
        if (!pool) {
            throw new Error('Database connection not established');
        }
        console.log('Pool exists, executing query...');
        const result = await pool.request().query('SELECT * FROM stores');
        console.log('Query result:', result);
        if (result && result.recordset) {
            console.log('Query successful, rows:', result.recordset);
            res.json(result.recordset);
        } else {
            throw new Error('No recordset returned');
        }
    } catch (error) {
        console.error('MSSQL Error:', error.message);
        console.error('Full error:', error);
        res.status(500).json({
            message: 'Server error',
            details: error.message,
            stack: error.stack
        });
    }
});

// Health check
app.get('/api/ping', (req, res) => {
    res.json({ status: 'ok' });
});

// Get attendance by store
app.get('/api/attendance/store/:storeId', authenticateToken, async (req, res) => {
    try {
        const { storeId } = req.params;
        const { date } = req.query;

        const result = await pool.request()
            .input('date', sql.Date, date)
            .input('storeId', sql.Int, storeId)
            .query(`SELECT 
                e.UserName AS EmployeeName,
                s.StoreName,
                a.Date,
                sh.DayOff,
                CASE WHEN sh.DayOff = 1 THEN 'İstirahət' ELSE '' END AS ShiftDisplay,
                sh.StartTime AS ShiftStart,
                sh.EndTime AS ShiftEnd,
                a.CheckIn,
                a.CheckOut
            FROM employees e
            LEFT JOIN shifts sh ON sh.EmployeeId = e.EmployeeId AND CAST(sh.Date AS DATE) = @date
            LEFT JOIN stores s ON s.StoreId = sh.StoreId
            LEFT JOIN attendance a ON a.EmployeeId = e.EmployeeId AND CAST(a.Date AS DATE) = @date
            WHERE s.StoreId = @storeId OR @storeId = 0
            ORDER BY e.EmployeeId`);
        res.json(result.recordset);
    } catch (error) {
        console.error(error);
        res.status(500).json({ message: 'Server error' });
    }
});

const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});