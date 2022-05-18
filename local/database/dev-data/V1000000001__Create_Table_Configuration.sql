-- Table: public.Configuration

-- DROP TABLE IF EXISTS public."Configuration";

CREATE TABLE IF NOT EXISTS public."Configuration"
(
    "Id" 			uuid NOT NULL,
    "Name" 			character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "Value" 		character varying(1000) COLLATE pg_catalog."default" NOT NULL,
    "Description" 	character varying(1000) COLLATE pg_catalog."default" NOT NULL,
	"StartDate" 	timestamp with time zone NOT NULL,
	"FinalDate" 	timestamp with time zone NOT NULL,
	"CreatedAt" 	timestamp with time zone NOT NULL,
    "LastUpdateAt" 	timestamp with time zone,
    "DeletedAt" 	timestamp with time zone
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Configuration"
    OWNER to postgres;
-- Index: Id_index

-- DROP INDEX IF EXISTS public."Id_index";

CREATE UNIQUE INDEX IF NOT EXISTS "Id_index"
    ON public."Configuration" USING btree
    ("Id" ASC NULLS LAST)
    INCLUDE("Id")
    TABLESPACE pg_default;