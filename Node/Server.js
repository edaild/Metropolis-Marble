const express = require('express');
const mysql = require('mysql2/promise');
const PORT = 3000;

const app = express();

const pool = mysql.createPool({
    host : 'localhost',
    name : 'root',
    password : '1234',
    database : 'Metropolis'

});

app.use(express.json());

app.get('/', (req,res)=>{
    res.send("root 경로에 서버가 성공적으로 연결되 있습니다.");
})

app.listen(PORT, ()=>{
    console.log("서버 실행중");
});