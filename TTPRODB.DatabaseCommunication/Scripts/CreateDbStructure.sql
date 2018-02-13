CREATE TABLE Items
(
    ID int NOT NULL PRIMARY KEY,
    Name text NOT NULL,
	Url text NOT NULL,
    Producer_ID int NOT NULL,
	Ratings int NOT NULL,
	Price int
);

CREATE TABLE Producers
(
    ID int NOT NULL PRIMARY KEY,
    Name text NOT NULL	
);

CREATE TABLE Rubbers
(
    ID int NOT NULL PRIMARY KEY,
    Item_ID int NOT NULL,
	Speed real,
    Spin real,
    Control real,
    Tackiness real,
    Weight real,
    Hardness real,
    Gears real,
    Throw_Angle real,
    Consistency real,
    Durability real,
    Overall real,
    Tensor bit,
    Anti bit,
);

CREATE TABLE Blades
(
    ID int NOT NULL PRIMARY KEY,
    Item_ID int NOT NULL,
	Speed int NOT NULL,
	Control real NOT NULL,
	Stiffness real NOT NULL,
	Hardness real NOT NULL,
	Consistency real NOT NULL,
	Overall real NOT NULL
);

CREATE TABLE Pips
(
    ID int NOT NULL PRIMARY KEY,
    Item_ID int NOT NULL,
	Speed real,
    Spin real,
    Control real,
    Deception real,
    Reversal real,
    Weight real,
    Hardness real,
    Consistency real,
    Durability real,
    Overall real,
    Type text
);