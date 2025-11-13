const dotenv = require('dotenv');
const path = require('path');
dotenv.config({ path: path.resolve(__dirname, '.env') });

const express = require('express');
const cors = require('cors');
const sql = require('mssql');
const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');
const { unwatchFile } = require('fs');
const { error } = require('console');

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
        const validPassword = password.trim() === employee.PassWordHash.trim();

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

app.get('/api/employees', async (req,res)=>{
        try {
            console.log('trying to excute employee query...');
            if (!pool){
                throw new Error ('Database connection not established');
            }
            console.log('pool exists,executing query...');
            const result= await pool.request().query('select * from employees');
            console.log('Query result:',result);
            if(result&& result.recordset){
                console.log('Query successful,rows:',result.recordset);
                res.json(result.recordset);
            }else {
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



app.get('/api/shifts/today/:employeeId', async (req, res) => {
  const { employeeId } = req.params;

  try {
    const result = await pool.request()
      .input('EmployeeId', sql.Int, employeeId)
      .query(`
        SELECT TOP 1 
          e.FullName,
          s.StoreName,
          sh.StartTime,
          sh.EndTime,
          sh.DayOff
        FROM Shifts sh
        INNER JOIN Stores s ON s.StoreId = sh.StoreId
        INNER JOIN Employees e ON e.EmployeeId = sh.EmployeeId
        WHERE sh.EmployeeId = @EmployeeId
          AND CAST(sh.Date AS DATE) = CAST(GETDATE() AS DATE)
        ORDER BY sh.Date DESC;
      `);

    if (result.recordset.length === 0) {
      return res.status(404).json({ message: 'Bugünkü növbə tapılmadı' });
    }

    const shift = result.recordset[0];

    const formatTime = (value) => {
      if (!value) return null;
      if (typeof value === 'string') return value.substring(0, 5);

      if (value instanceof Date) {
        value.setHours(value.getHours() - 4); // 🇦🇿 UTC+4 düzəlişi
        return value.toTimeString().substring(0, 5);
      }
      return null;
    };

    res.json({
      FullName: shift.FullName,
      StoreName: shift.StoreName,
      StartTime: formatTime(shift.StartTime),
      EndTime: formatTime(shift.EndTime),
      DayOff: shift.DayOff
    });
  } catch (error) {
    console.error('❌ Shift query error:', error);
    res.status(500).json({ error: 'Server error', details: error.message });
  }
});



app.get('/api/attendance/all', async (req, res) => {
    try {
        if (!pool) {
            return res.status(500).json({ error: 'Database not connected yet' });
        }

        const result = await pool.request().query(`
            SELECT 
                e.UserName AS EmployeeName,
                s.StoreName,
                CASE 
                    WHEN a.Date IS NOT NULL THEN CAST(a.Date AS DATE)
                    ELSE CAST(GETDATE() AS DATE)
                END AS Date,
                ISNULL(sh.DayOff, 0) AS DayOff, 
                CASE
                    WHEN sh.DayOff = 1 THEN N'İstirahət' ELSE ''
                END AS ShiftDisplay,
                sh.StartTime AS ShiftStart,
                sh.EndTime AS ShiftEnd,
                a.CheckIn,
                a.CheckOut
            FROM Employees e
            LEFT JOIN Shifts sh 
                ON sh.EmployeeId = e.EmployeeId
                AND CAST(sh.Date AS DATE) = CAST(GETDATE() AS DATE)
            LEFT JOIN Stores s 
                ON s.StoreId = sh.StoreId
            LEFT JOIN Attendance a 
                ON a.EmployeeId = e.EmployeeId
                AND (a.StoreId = s.StoreId OR a.StoreId IS NULL)
                AND a.Date = sh.Date
            ORDER BY e.EmployeeId
        `);

        // Məlumatları qaytarırıq
        const data = result.recordset.map(row => ({
            EmployeeName: row.EmployeeName,
            StoreName: row.StoreName,
            Date: row.Date,
            DayOff: row.DayOff,
            ShiftDisplay: row.ShiftDisplay,
            ShiftStart: row.ShiftStart ? `${row.ShiftStart.getHours().toString().padStart(2,'0')}:${row.ShiftStart.getMinutes().toString().padStart(2,'0')}` : null,
            ShiftEnd: row.ShiftEnd ? `${row.ShiftEnd.getHours().toString().padStart(2,'0')}:${row.ShiftEnd.getMinutes().toString().padStart(2,'0')}` : null,
            CheckIn: row.CheckIn,
            CheckOut: row.CheckOut
        }));

        res.json(data);
    } catch (error) {
        console.error('❌ Error fetching all employee attendance:', error);
        res.status(500).json({ error: 'Server error' });
    }
});


app.get('/api/attendance/store/:storeId/date/:selectedDate', async (req, res) => {
  const { storeId, selectedDate } = req.params;

  try {
    const result = await pool.request()
      .input('StoreId', sql.Int, parseInt(storeId))
      .input('SelectedDate', sql.Date, new Date(selectedDate))
      .query(`
          SELECT 
              e.EmployeeId,
              e.UserName AS EmployeeName,
              s.StoreId,
              s.StoreName,
              CASE 
                  WHEN a.Date IS NOT NULL THEN CAST(a.Date AS DATE)
                  WHEN sh.DayOff = 1 THEN @SelectedDate
                  ELSE @SelectedDate
              END AS Date,
              ISNULL(sh.DayOff, 0) AS DayOff,
              CASE
                WHEN sh.DayOff = 1 THEN 'İstirahət' ELSE ''
            END AS ShiftDisplay,
              sh.StartTime AS ShiftStart,
              sh.EndTime AS ShiftEnd,
              a.CheckIn,
              a.CheckOut
          FROM Employees e
          LEFT JOIN Shifts sh 
              ON sh.EmployeeId = e.EmployeeId
             AND CAST(sh.Date AS DATE) = @SelectedDate
          LEFT JOIN Stores s 
              ON s.StoreId = sh.StoreId
          LEFT JOIN Attendance a 
              ON a.EmployeeId = e.EmployeeId
             AND CAST(a.Date AS DATE) = @SelectedDate
          WHERE 
              (@StoreId = 0 OR s.StoreId = @StoreId OR s.StoreId IS NULL)
          ORDER BY e.EmployeeId, sh.StartTime
      `);

    // SQL Server TIME tipini JSON-da "HH:mm:ss" şəklində göndər
    const attendanceData = result.recordset.map(r => ({
      ...r,
      ShiftStart: r.ShiftStart ? r.ShiftStart.toISOString().substr(11, 8) : null,
      ShiftEnd: r.ShiftEnd ? r.ShiftEnd.toISOString().substr(11, 8) : null
    }));

    res.json(attendanceData);
  } catch (err) {
    console.error('Error fetching attendance:', err);
    res.status(500).json({ error: 'Server error' });
  }
});

app.get('/api/attendance/store/:storeId', async (req, res) => {
    const storeId = parseInt(req.params.storeId);
    const selectedDate = req.query.date ? new Date(req.query.date) : new Date();

    try {
        const pool = await sql.connect(config);

        const query = `
            SELECT 
                e.EmployeeId,
                e.UserName AS EmployeeName,
                s.StoreId,
                s.StoreName,
                CASE 
                    WHEN a.Date IS NOT NULL THEN CAST(a.Date AS DATE)
                    WHEN sh.DayOff = 1 THEN @SelectedDate
                    ELSE CAST(GETDATE() AS DATE)
                END AS Date,
                ISNULL(sh.DayOff, 0) AS DayOff,
                CASE WHEN sh.DayOff = 1 THEN N'İstirahət' ELSE '' END AS ShiftDisplay,
                FORMAT(sh.StartTime, 'HH:mm') AS ShiftStart,
                FORMAT(sh.EndTime, 'HH:mm') AS ShiftEnd,
                a.CheckIn,
                a.CheckOut
            FROM Employees e
            LEFT JOIN Shifts sh 
                ON sh.EmployeeId = e.EmployeeId
                AND CAST(sh.Date AS DATE) = CAST(@SelectedDate AS DATE)
            LEFT JOIN Stores s 
                ON s.StoreId = sh.StoreId
            LEFT JOIN Attendance a 
                ON a.EmployeeId = e.EmployeeId
                AND CAST(a.Date AS DATE) = CAST(@SelectedDate AS DATE)
            WHERE (@StoreId = 0 OR s.StoreId = @StoreId OR s.StoreId IS NULL)
            ORDER BY e.EmployeeId, sh.StartTime;
        `;

        const result = await pool.request()
            .input('StoreId', sql.Int, storeId)
            .input('SelectedDate', sql.Date, selectedDate)
            .query(query);

        // JSON formatına çevirmək (C# DTO uyğunluğu üçün)
        const data = result.recordset.map(row => ({
            employeeId: row.EmployeeId,
            employeeName: row.EmployeeName,
            storeId: row.StoreId,
            storeName: row.StoreName,
            date: row.Date,
            dayOff: !!row.DayOff,
            ShiftStart: r.ShiftStart ? r.ShiftStart.toISOString().substr(11, 8) : null,
            ShiftEnd: r.ShiftEnd ? r.ShiftEnd.toISOString().substr(11, 8) : null,
            checkIn: row.CheckIn,
            checkOut: row.CheckOut,
            shiftDisplay: row.DayOff
                ? 'İstirahət'
                : `${row.ShiftStart} - ${row.ShiftEnd}`
        }));

        res.json(data);

    } catch (err) {
        console.error('❌ Database error:', err);
        res.status(500).json({ error: 'Internal Server Error' });
    }
});


app.get('/api/attendance', async (req, res) => {
  try {
    const storeId = req.query.storeId ? parseInt(req.query.storeId) : 0;

    let selectedDate;
    if (req.query.date) {
      // 🔹 UTC tarix yaratmaq üçün toISOString istifadə edirik
      const [year, month, day] = req.query.date.split('-');
      selectedDate = new Date(Date.UTC(Number(year), Number(month) - 1, Number(day)));
    } else {
      const now = new Date();
      selectedDate = new Date(Date.UTC(now.getFullYear(), now.getMonth(), now.getDate()));
    }

    console.log("📅 SelectedDate (UTC):", selectedDate);

    const query = `
      SELECT 
          e.EmployeeId,
          e.UserName AS EmployeeName,
          s.StoreId,
          s.StoreName,
          CASE 
              WHEN a.Date IS NOT NULL THEN CAST(a.Date AS DATE)
              ELSE @SelectedDate
          END AS Date,
          ISNULL(sh.DayOff, 0) AS DayOff,
          CONVERT(varchar(5), sh.StartTime, 108) AS ShiftStart,
          CONVERT(varchar(5), sh.EndTime, 108) AS ShiftEnd,
          a.CheckIn,
          a.CheckOut
      FROM Employees e
      LEFT JOIN Shifts sh 
          ON sh.EmployeeId = e.EmployeeId
          AND CAST(sh.Date AS DATE) = CAST(@SelectedDate AS DATE)
      LEFT JOIN Stores s 
          ON s.StoreId = sh.StoreId
      LEFT JOIN Attendance a 
          ON a.EmployeeId = e.EmployeeId
          AND CAST(a.Date AS DATE) = CAST(@SelectedDate AS DATE)
      WHERE (@StoreId = 0 OR s.StoreId = @StoreId)
      ORDER BY e.EmployeeId, sh.StartTime;
    `;

    const result = await pool.request()
      .input('StoreId', sql.Int, storeId)
      .input('SelectedDate', sql.Date, selectedDate)
      .query(query);

    const data = result.recordset.map(row => ({
      employeeId: row.EmployeeId,
      employeeName: row.EmployeeName,
      storeId: row.StoreId,
      storeName: row.StoreName,
      date: row.Date,
      dayOff: !!row.DayOff,
      shiftStart: row.ShiftStart,
      shiftEnd: row.ShiftEnd,
      checkIn: row.CheckIn,
      checkOut: row.CheckOut,
      shiftDisplay: row.DayOff ? 'İstirahət' : `${row.ShiftStart} - ${row.ShiftEnd}`
    }));

    res.json(data);
  } catch (err) {
    console.error('❌ Error:', err);
    res.status(500).json({ message: 'Internal Server Error' });
  }
});


app.get('/api/attendance/search', async (req, res) => {
  try {
    const storeId = req.query.storeId ? parseInt(req.query.storeId) : 0;
    const searchText = req.query.search ? req.query.search.trim() : '';


     let selectedDate;
    if (req.query.date) {
      // 🔹 UTC tarix yaratmaq üçün toISOString istifadə edirik
      const [year, month, day] = req.query.date.split('-');
      selectedDate = new Date(Date.UTC(Number(year), Number(month) - 1, Number(day)));
    } else {
      const now = new Date();
      selectedDate = new Date(Date.UTC(now.getFullYear(), now.getMonth(), now.getDate()));
    }

    console.log("📅 SelectedDate (UTC):", selectedDate); 
    //const pool = await sql.connect(config);

    const query = `
      SELECT 
          e.EmployeeId,
          e.UserName AS EmployeeName,
          s.StoreId,
          s.StoreName,
          CASE 
              WHEN a.Date IS NOT NULL THEN CAST(a.Date AS DATE)
              WHEN sh.DayOff = 1 THEN @SelectedDate
              ELSE @SelectedDate
          END AS Date,
          ISNULL(sh.DayOff, 0) AS DayOff, 
          CASE
              WHEN sh.DayOff = 1 THEN N'İstirahət' ELSE ''
          END AS ShiftDisplay,
          CONVERT(varchar(5), sh.StartTime, 108) AS ShiftStart,
          CONVERT(varchar(5), sh.EndTime, 108) AS ShiftEnd,
          a.CheckIn,
          a.CheckOut
      FROM Employees e
      LEFT JOIN Shifts sh 
          ON sh.EmployeeId = e.EmployeeId
          AND CAST(sh.Date AS DATE) = CAST(@SelectedDate AS DATE)
      LEFT JOIN Stores s 
          ON s.StoreId = sh.StoreId
      LEFT JOIN Attendance a 
          ON a.EmployeeId = e.EmployeeId
          AND CAST(a.Date AS DATE) = CAST(@SelectedDate AS DATE)
      WHERE 
          (@StoreId = 0 OR s.StoreId = @StoreId OR s.StoreId IS NULL)
          AND (e.UserName LIKE '%' + @SearchText + '%')
      ORDER BY e.EmployeeId, sh.StartTime;
    `;

    const result = await pool.request()
      .input('StoreId', sql.Int, storeId)
      .input('SelectedDate', sql.Date, selectedDate)
      .input('SearchText', sql.NVarChar, searchText)
      .query(query);

    const data = result.recordset.map(row => ({
      employeeId: row.EmployeeId,
      employeeName: row.EmployeeName,
      storeId: row.StoreId,
      storeName: row.StoreName,
      date: row.Date,
      dayOff: !!row.DayOff,
      shiftStart: row.ShiftStart,
      shiftEnd: row.ShiftEnd,
      checkIn: row.CheckIn,
      checkOut: row.CheckOut,
      shiftDisplay: row.ShiftDisplay || 
        (row.DayOff ? 'İstirahət' : `${row.ShiftStart || ''} - ${row.ShiftEnd || ''}`)
    }));

    res.json(data);

  } catch (err) {
    console.error('❌ Error:', err);
    res.status(500).json({ message: 'Internal Server Error' });
  }
});




const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});