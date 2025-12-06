require('dotenv').config();
const express = require('express');
const mysql = require('mysql2/promise');
const bodyParser = require('body-parser');
const bcrypt = require('bcrypt');

const PORT = 3002 || process.env.PORT;
const SALT_ROUNDS = 10;

const app = express();
app.use(bodyParser.json());

const pool = mysql.createPool({
    host : 'localhost',
    user : 'root',
    password : '1234',
    database : 'MniGTA'
});

app.use(express.json());

app.post('/membership', async(req, res) =>{
    const { useremail, userpassword, username } = req.body;

    if (!useremail || !userpassword || !username) {
        return res.status(400).json({ success: false, message: '이메일, 비밀번호, 사용자 이름을 모두 제공해야 합니다.' });
    }

    try
    { 
            const[existingPlayers] =  await pool.query(
            'select * from players where player_email = ?',
             [useremail]
            );

        if(existingPlayers > 0)
        {
            return res.status(409).json({ success: false, message: "이미 존재하는 이메일 입니다." });
        } 

        const hashedPassword = await bcrypt.hash(userpassword, SALT_ROUNDS);

        const [result] = await pool.query(
            'INSERT INTO players (player_email, player_password, player_name, player_level, current_money) VALUES (?, ?, ?, 1, 1000);',
            [useremail, hashedPassword, username]
        );

        res.status(201).json({ success: true, message: '회원 가입 성공', userId: result.insertId });
    }
    catch
    {
          res.status(500).json({success : false , message : "서버 에러 발생"});
    }
});

app.listen(PORT, ()=>{
    console.log("회원가입 서버 실행중: http://localhost:${PORT}");
});
