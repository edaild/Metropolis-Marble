const express = require('express');
const mysql = require('mysql2/promise');
const PORT = 3000;

const app = express();
let isNullData = false;

const pool = mysql.createPool({
    host : 'localhost',
    user : 'root',
    password : '1234',
    database : 'MetroPolis'

});

app.use(express.json());

app.post('/membership', async(req, res) =>{
    const [useremail, userpassword, username] = req.body;
    try
    {
        const [useremail] = req.body;
        
            const[playre] =  await pool.query(
            'select * from players where player_email = ?',
             isNullData = true
            );
       
       
        if(isNullData){
            const[Player] = await pool.query(
                'insert into players (player_email, player_password, player_name, player_character_id, player_level)value("?","?","?","1","1")',
                res.status(200).json({success : true, message : '회원 가입 성공'})
            );
        }
        else
        {
             res.status(500).json({success : false , message : "이미 존재하는 이메일 입니다."});
        }
    }
    catch
    {
          res.status(500).json({success : false , message : "서버 에러 발생"});
    }
});

app.listen(PORT, ()=>{
    console.log("회원가입 서버 실행중");
});
