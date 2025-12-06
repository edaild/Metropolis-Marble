const express = require('express');
const mysql = require('mysql2/promise');

const PORT = 3000;

const app = express();

const pool = mysql.createPool({
    host : 'localhost',
    user : 'root',
    password : '1234',
    database : 'MiniGTA'

});

app.use(express.json());

app.get('/weapon_types', async(req, res) =>{
    try
    {
        const[weapon_types] = await pool.query(
            "SELECT weapon_type_id, weapon_name, base_damage, ammo_type FROM weapon_types",
        );
        res.status(200).json(weapon_types);
    }
    catch
    {
        res.status(500).json({success : false , message : "무기 경로 서버 에러 발생"});
    }
});

app.get('/npc_character', async(req, res) =>{
    try
    {
        const[npc_types] = await pool.query(
            "select npc_type_id, npc_name, is_hostile, base_health, base_damage from npc_types",
        );
         res.status(200).json(npc_types);
    }
    catch
    {
         res.status(500).json({success : false , message : "NPC 캐릭터 경로 서버 에러 발생"});
    }
});

app.get('/shop', async(req, res) =>{
    try
    {
         const[shop] = await pool.query(
            "SELECT shop_id, w.weapon_name gun_name,  transaction_price, base_damage FROM shop s JOIN weapon_types w on s.gun_id = w.weapon_type_id",
        );
        res.status(200).json(shop);
    }
    catch
    {
         res.status(500).json({success : false , message : "상점 경로 서버 에러 발생"});
    }
})

app.get('/', (req,res)=>{
    res.send("root 경로에 서버가 성공적으로 연결되 있습니다.");
})

app.listen(PORT, ()=>{
    console.log("고정형 데이터 베이스 서버 실행중");
});