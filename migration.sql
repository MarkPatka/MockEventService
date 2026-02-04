CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE "Events" (
    "EventId" uuid NOT NULL,
    "Title" character varying(100) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "EventTypeName" character varying(100) NOT NULL,
    "EventTypeDescription" character varying(500),
    "EventTypeIcon" character varying(200),
    "EventTypeId" uuid NOT NULL,
    "LocationAddress" character varying(200) NOT NULL,
    "LocationCity" character varying(100) NOT NULL,
    "LocationCountry" character varying(100) NOT NULL,
    "LocationLatitude" numeric(10,7),
    "LocationLongitude" numeric(10,7),
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone NOT NULL,
    "MaxParticipants" integer NOT NULL,
    "Status" character varying(50) NOT NULL,
    "OrganizerId" uuid NOT NULL,
    "OrganizerName" character varying(100) NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "ReviewsCount" integer NOT NULL DEFAULT 0,
    "AverageRating" numeric(3,2),
    CONSTRAINT "PK_Events" PRIMARY KEY ("EventId")
);

CREATE TABLE "EventStatuses" (
    "EventStatusId" integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Description" text,
    CONSTRAINT "PK_EventStatuses" PRIMARY KEY ("EventStatusId")
);

CREATE TABLE "ParticipantStatuses" (
    "ParticipantStatusId" integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    "Description" text,
    CONSTRAINT "PK_ParticipantStatuses" PRIMARY KEY ("ParticipantStatusId")
);

CREATE TABLE "Participants" (
    "ParticipantId" uuid NOT NULL,
    "EventId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "UserName" character varying(100) NOT NULL,
    "RegisteredAt" timestamp with time zone NOT NULL,
    "Status" character varying(50) NOT NULL,
    CONSTRAINT "PK_Participants" PRIMARY KEY ("ParticipantId"),
    CONSTRAINT "FK_Participants_Events_EventId" FOREIGN KEY ("EventId") REFERENCES "Events" ("EventId") ON DELETE CASCADE
);

INSERT INTO "EventStatuses" ("EventStatusId", "Description", "Name")
VALUES (1, 'Draft', 'Draft');
INSERT INTO "EventStatuses" ("EventStatusId", "Description", "Name")
VALUES (2, 'Active', 'Active');
INSERT INTO "EventStatuses" ("EventStatusId", "Description", "Name")
VALUES (3, 'Cancelled', 'Cancelled');
INSERT INTO "EventStatuses" ("EventStatusId", "Description", "Name")
VALUES (4, 'Finished', 'Completed');

INSERT INTO "ParticipantStatuses" ("ParticipantStatusId", "Description", "Name")
VALUES (1, 'Зарегистрирован', 'Registered');
INSERT INTO "ParticipantStatuses" ("ParticipantStatusId", "Description", "Name")
VALUES (2, 'Присутствовал', 'Attended');
INSERT INTO "ParticipantStatuses" ("ParticipantStatusId", "Description", "Name")
VALUES (3, 'Отменил участие', 'Cancelled');

CREATE INDEX "IX_Events_OrganizerId" ON "Events" ("OrganizerId");

CREATE INDEX "IX_Events_OrganizerId_Status" ON "Events" ("OrganizerId", "Status");

CREATE INDEX "IX_Events_StartDate" ON "Events" ("StartDate");

CREATE INDEX "IX_Events_Status" ON "Events" ("Status");

CREATE INDEX "IX_Events_Status_StartDate" ON "Events" ("Status", "StartDate");

CREATE UNIQUE INDEX "IX_Participants_EventId_UserId" ON "Participants" ("EventId", "UserId");

CREATE INDEX "IX_Participants_Status" ON "Participants" ("Status");

CREATE INDEX "IX_Participants_UserId" ON "Participants" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260204125140_InitialCreate', '10.0.2');

COMMIT;

