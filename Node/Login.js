require('dotenv').config();
const express = require('express');
const mysql = require('mysql2/promise');
const bodyParser = require('body-parser');
const jwt = require('jsonwebtoken');
const bcrypt = require('bcrypt');

const app = express();
app.use(bodyParser.json());
app.use(express.json());

const JWT_SECRET = process.env.JWT_SECRET;
const REFRESH_TOKEN_SECRET = process.env.REFRESH_TOKEN_SECRET;
const PORT = process.env.PORT || 3001;

const pool = mysql.createPool({
    host: 'localhost',
    user: 'root',
    password: '1234',
    database: 'MiniGTA'
});

const generateTokens = (playerId) => {
    const accessToken = jwt.sign({ id: playerId }, JWT_SECRET, { expiresIn: '1h' });
    const refreshToken = jwt.sign({ id: playerId }, REFRESH_TOKEN_SECRET, { expiresIn: '7d' });
    return { accessToken, refreshToken };
};

app.post('/user', async (req, res) => {
    const { useremail, userpassword } = req.body;

    if (!useremail || !userpassword) {
        return res.status(400).json({ success: false, message: "이메일과 비밀번호를 모두 입력해야 합니다." });
    }

    try {
        const [rows] = await pool.query(
            'SELECT player_id, player_email, player_password FROM players WHERE player_email = ?',
            [useremail]
        );

        const player = rows[0];

        if (!player) {
            return res.status(401).json({ success: false, message: "사용자를 찾을 수 없습니다." });
        }

        const isMatch = await bcrypt.compare(userpassword, player.player_password);
        
        if (!isMatch) {
            return res.status(401).json({ success: false, message: "비밀번호가 일치하지 않습니다." });
        }

        await pool.query(
            'UPDATE players SET death_time = CURRENT_TIMESTAMP WHERE player_id = ?',
            [player.player_id]
        );

        const { accessToken, refreshToken } = generateTokens(player.player_id);

        res.status(200).json({ 
            success: true, 
            message: "로그인 성공", 
            accessToken, 
            refreshToken 
        });

    } catch (error) {
        console.error("로그인 서버 에러 발생:", error);
        res.status(500).json({ success: false, message: "로그인 서버 에러 발생" });
    }
});

app.get('/', (req, res) => {
    res.send("root 경로에 서버가 성공적으로 연결되 있습니다.");
});

app.listen(PORT, () => {
    console.log(`로그인 서버 실행중: http://localhost:${PORT}`);
});