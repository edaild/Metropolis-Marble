use minigta;
CREATE TABLE `players` (
  `player_id` INT PRIMARY KEY AUTO_INCREMENT,
  `player_email` VARCHAR(255) UNIQUE NOT NULL,
  `player_password` VARCHAR(255) NOT NULL,
  `player_name` VARCHAR(32) NOT NULL,
  `player_level` INT DEFAULT 1,
  `current_money` INT DEFAULT 0,
  `is_dead` BOOLEAN DEFAULT FALSE,
  `death_time` TIMESTAMP NULL,
  `respawn_cost` INT DEFAULT 0
);

INSERT INTO players (player_email, player_password, player_name, player_level, current_money) VALUES 
('test@gmail.com', '1234', 'testplayer', '1','1000');

select * from players;

CREATE TABLE `weapon_types` (
  `weapon_type_id` INT PRIMARY KEY AUTO_INCREMENT,
  `weapon_name` VARCHAR(32) NOT NULL,
  `base_damage` INT NOT NULL,
  `ammo_type` VARCHAR(32)
);


INSERT INTO `weapon_types` (`weapon_name`, `base_damage`, `ammo_type`) VALUES
('권총', 15, 'Small Caliber'),
('샷권', 40, 'Shell'),
('소총', 30, 'Large Caliber');

CREATE TABLE `player_inventory` (
  `user_Inventory_id` INT PRIMARY KEY AUTO_INCREMENT,
  `player_id` INT NOT NULL,
  `weapon_type_id` INT NOT NULL,
  `item_level` INT DEFAULT 1,
  `item_count` INT DEFAULT 1
);

CREATE TABLE `npc_types` (
  `npc_type_id` INT PRIMARY KEY AUTO_INCREMENT,
  `npc_name` VARCHAR(32) NOT NULL,
  `is_hostile` BOOLEAN NOT NULL,
  `base_health` INT,
  `base_damage` INT
);

-- 시민 NPC
INSERT INTO npc_types (npc_name, is_hostile, base_health, base_damage) VALUES 
('시민1', FALSE, 1000, 0), 
('시민2', FALSE, 1000, 0), 
('시민3', FALSE, 1000, 0), 
('시민4', FALSE, 1000, 0),
('시민5', FALSE, 1000, 0); 

-- 경찰 NPC
INSERT INTO npc_types (npc_name, is_hostile, base_health, base_damage) VALUES 
('일반경찰', true, 1000, 50), 
('강력팀 경찰', true, 1000, 100), 
('경찰 특공대', true, 1000, 150);

select npc_type_id, npc_name, is_hostile, base_health, base_damage from npc_types;


CREATE TABLE `shop` (
  `shop_id` INT PRIMARY KEY AUTO_INCREMENT,
  `gun_id` INT NULL,
  `transaction_price` INT NOT NULL,
  `transaction_date` DATETIME
);
INSERT INTO `shop` (`gun_id`, `transaction_price`) VALUES
(1, 1000), -- Pistol 1000원
(2, 3500), -- Shotgun 3500원
(3, 2500); -- Rifle 2500원

drop table shop;
ALTER TABLE `player_inventory`
ADD CONSTRAINT `fk_inventory_player`
FOREIGN KEY (`player_id`) REFERENCES `players` (`player_id`);

ALTER TABLE `player_inventory`
ADD CONSTRAINT `fk_inventory_weapon_type`
FOREIGN KEY (`weapon_type_id`) REFERENCES `weapon_types` (`weapon_type_id`);


ALTER TABLE `shop`
ADD CONSTRAINT `fk_shop_weapon_type`
FOREIGN KEY (`gun_id`) REFERENCES `weapon_types` (`weapon_type_id`);

select * from players;
SELECT shop_id, w.weapon_name gun_name,  transaction_price, base_damage FROM shop s JOIN weapon_types w on s.gun_id = w.weapon_type_id