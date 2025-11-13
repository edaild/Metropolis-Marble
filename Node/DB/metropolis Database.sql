create database MetroPolis;
use MetroPolis;
-- 플레이어
create table players(
	player_id int auto_increment primary key,
    player_email varchar(255) not null,
    player_password varchar(255) not null,
    player_name varchar(255) not null,
    player_character_id int not null,
    player_level int not null,
    player_last_login_time timestamp null default null,
	foreign key(player_character_id) references CharacterCard(character_id)
) auto_increment = 10000;

-- 캐릭터 카드
create table characterCard(
	character_id int auto_increment primary key,
    character_name varchar(32) not null,
    character_description varchar(500) not null
);

insert into characterCard(character_name, character_description) value
('세레나','차분하고 우아한 리더 타입으로, 겉보기엔 완벽해 보이지만 누구보다 노력으로 자신을 빛내는 소녀.'),
('릴리에','순수하고 포근한 감성의 소녀로, 작은 친절에도 행복을 느끼는 따뜻한 마음이 주변을 부드럽게 만든다.'),
('미나','쿨하고 세련된 트렌드세터로, 도시의 리듬 속에서도 자기만의 색을 잃지 않는 자신감이 매력적인 소녀.'),
('셀리아','열정적이고 자유로운 영혼의 마술사로, 빛과 환상을 다루듯 무대 위에서 언제나 눈부신 퍼포먼스를 보여준다.'),
('루루','밝고 귀엽고 스타일리시한 아이돌 지망생으로, 아직 데뷔 전이지만 꿈을 향한 열정과 자신감이 빛나는 소녀.');

-- 상점
create table shop( 
	shop_id int auto_increment key,
    charactgercard_id int not null,
    character_price int not null,
    charactercard_count int not null,
	foreign key(charactgercard_id) references CharacterCard(character_id)
);

insert into shop(charactgercard_id,character_price,charactercard_count)value
('1','100','1'),
('2','100','1'),
('3','100','1'),
('4','100','1'),
('5','100','1');





