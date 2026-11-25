CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Name" character varying(256) NOT NULL,
    "Email" text NOT NULL,
    "Balance" numeric NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Projects" (
    "Id" uuid NOT NULL,
    "AuthorId" uuid NOT NULL,
    "Title" character varying(256) NOT NULL,
    "Description" character varying(1024) NOT NULL,
    "Threshold" numeric NOT NULL,
    "Status" text NOT NULL,
    CONSTRAINT "PK_Projects" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Projects_Users_AuthorId" FOREIGN KEY ("AuthorId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Rewards" (
    "Id" uuid NOT NULL,
    "ProjectId" uuid NOT NULL,
    "Title" character varying(256) NOT NULL,
    "Amount" numeric NOT NULL,
    CONSTRAINT "PK_Rewards" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Rewards_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Pledges" (
    "Id" uuid NOT NULL,
    "ProjectId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "RewardId" uuid,
    "Amount" numeric NOT NULL,
    "Status" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Pledges" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Pledges_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES "Projects" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Pledges_Rewards_RewardId" FOREIGN KEY ("RewardId") REFERENCES "Rewards" ("Id"),
    CONSTRAINT "FK_Pledges_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Pledges_ProjectId" ON "Pledges" ("ProjectId");

CREATE INDEX "IX_Pledges_RewardId" ON "Pledges" ("RewardId");

CREATE INDEX "IX_Pledges_UserId" ON "Pledges" ("UserId");

CREATE INDEX "IX_Projects_AuthorId" ON "Projects" ("AuthorId");

CREATE INDEX "IX_Rewards_ProjectId" ON "Rewards" ("ProjectId");

CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261008103233_Initial', '8.0.30');

COMMIT;

