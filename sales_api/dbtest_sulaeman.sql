--
-- PostgreSQL database dump
--

-- Dumped from database version 14.11 (Homebrew)
-- Dumped by pg_dump version 14.11 (Homebrew)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: generate_sales_order_no(); Type: FUNCTION; Schema: public; Owner: test
--

CREATE FUNCTION public.generate_sales_order_no() RETURNS trigger
    LANGUAGE plpgsql
AS $$
BEGIN
    NEW."SalesOrderNo" := CONCAT('SO', LPAD(nextval('sales_order_sequence')::text, 4, '0'));
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.generate_sales_order_no() OWNER TO test;

--
-- Name: insert_sales_order_interface(); Type: FUNCTION; Schema: public; Owner: test
--

CREATE FUNCTION public.insert_sales_order_interface() RETURNS trigger
    LANGUAGE plpgsql
AS $$
BEGIN
    -- Construct JSON object for the payload
    INSERT INTO "SalesOrderInterface" ("SalesOrderNo", "Payload")
    VALUES (NEW."SalesOrderNo",
            jsonb_build_object(
                    'SalesOrderNo', NEW."SalesOrderNo",
                    'CustId', NEW."CustCode",
                    'OrderDetail', jsonb_build_array(
                            jsonb_build_object('ProductCode', NEW."ProductCode", 'Qty', NEW."Qty")
                                   )
            ));

    RETURN NEW;
END;
$$;


ALTER FUNCTION public.insert_sales_order_interface() OWNER TO test;

--
-- Name: insertsalesorder(timestamp, character varying, character varying, integer, numeric); Type: PROCEDURE; Schema: public; Owner: test
--

CREATE PROCEDURE public.insertsalesorder(IN p_order_date timestamp, IN p_cust_code character varying, IN p_product_code character varying, IN p_qty integer, IN p_price numeric)
    LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO "SalesOrder" ("OrderDate", "CustCode", "ProductCode", "Qty", "Price")
    VALUES (p_order_date, p_cust_code, p_product_code, p_qty, p_price);
END;
$$;


ALTER PROCEDURE public.insertsalesorder(IN p_order_date timestamp, IN p_cust_code character varying, IN p_product_code character varying, IN p_qty integer, IN p_price numeric) OWNER TO test;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Customer; Type: TABLE; Schema: public; Owner: test
--

CREATE TABLE public."Customer" (
                                   "CustId" integer NOT NULL,
                                   "CustName" character varying(50)
);


ALTER TABLE public."Customer" OWNER TO test;

--
-- Name: Customer_CustId_seq; Type: SEQUENCE; Schema: public; Owner: test
--

CREATE SEQUENCE public."Customer_CustId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Customer_CustId_seq" OWNER TO test;

--
-- Name: Customer_CustId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: test
--

ALTER SEQUENCE public."Customer_CustId_seq" OWNED BY public."Customer"."CustId";


--
-- Name: Price; Type: TABLE; Schema: public; Owner: test
--

CREATE TABLE public."Price" (
                                "PriceId" integer NOT NULL,
                                "ProductCode" character varying(10),
                                "Price" numeric,
                                "PriceValidateFrom" date,
                                "PriceValidateTo" date
);


ALTER TABLE public."Price" OWNER TO test;

--
-- Name: Price_PriceId_seq; Type: SEQUENCE; Schema: public; Owner: test
--

CREATE SEQUENCE public."Price_PriceId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."Price_PriceId_seq" OWNER TO test;

--
-- Name: Price_PriceId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: test
--

ALTER SEQUENCE public."Price_PriceId_seq" OWNED BY public."Price"."PriceId";


--
-- Name: Product; Type: TABLE; Schema: public; Owner: test
--

CREATE TABLE public."Product" (
                                  "ProductCode" character varying(10) NOT NULL,
                                  "ProductName" character varying(255)
);


ALTER TABLE public."Product" OWNER TO test;

--
-- Name: SalesOrder; Type: TABLE; Schema: public; Owner: test
--

CREATE TABLE public."SalesOrder" (
                                     "SalesOrderNo" character varying(10) NOT NULL,
                                     "OrderDate" timestamp NOT NULL,
                                     "CustCode" character varying(10),
                                     "ProductCode" character varying(10),
                                     "Qty" integer NOT NULL,
                                     "Price" numeric NOT NULL
);


ALTER TABLE public."SalesOrder" OWNER TO test;

--
-- Name: SalesOrderInterface; Type: TABLE; Schema: public; Owner: test
--

CREATE TABLE public."SalesOrderInterface" (
                                              "SalesOrderNo" character varying(10) NOT NULL,
                                              "Payload" json
);


ALTER TABLE public."SalesOrderInterface" OWNER TO test;

--
-- Name: sales_order_sequence; Type: SEQUENCE; Schema: public; Owner: test
--

CREATE SEQUENCE public.sales_order_sequence
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.sales_order_sequence OWNER TO test;

--
-- Name: Customer CustId; Type: DEFAULT; Schema: public; Owner: test
--

ALTER TABLE ONLY public."Customer" ALTER COLUMN "CustId" SET DEFAULT nextval('public."Customer_CustId_seq"'::regclass);


--
-- Name: Price PriceId; Type: DEFAULT; Schema: public; Owner: test
--

ALTER TABLE ONLY public."Price" ALTER COLUMN "PriceId" SET DEFAULT nextval('public."Price_PriceId_seq"'::regclass);


--
-- Data for Name: Customer; Type: TABLE DATA; Schema: public; Owner: test
--

COPY public."Customer" ("CustId", "CustName") FROM stdin;
1	Customer 1
2	Customer 2
3	Customer 3
\.


--
-- Data for Name: Price; Type: TABLE DATA; Schema: public; Owner: test
--

COPY public."Price" ("PriceId", "ProductCode", "Price", "PriceValidateFrom", "PriceValidateTo") FROM stdin;
2	st002	60000	2023-12-10	2024-08-01
1	st002	55000	2023-01-01	2024-11-12
3	eg001	111	2023-01-01	2024-08-01
4	eg002	34132	2024-01-01	2024-12-12
\.


--
-- Data for Name: Product; Type: TABLE DATA; Schema: public; Owner: test
--

COPY public."Product" ("ProductCode", "ProductName") FROM stdin;
st001	Torios
st002	Blue Controller
eg001	Tempo 4R
eg002	Steelbuds
\.


--
-- Data for Name: SalesOrder; Type: TABLE DATA; Schema: public; Owner: test
--

COPY public."SalesOrder" ("SalesOrderNo", "OrderDate", "CustCode", "ProductCode", "Qty", "Price") FROM stdin;
SO0015	2024-04-05 00:00:00	2	eg001	1	1.0
SO0016	2024-04-05 00:00:00	string	string	0	0
SO0017	2024-04-05 00:00:00	2	st001	12	0.0
\.


--
-- Data for Name: SalesOrderInterface; Type: TABLE DATA; Schema: public; Owner: test
--

COPY public."SalesOrderInterface" ("SalesOrderNo", "Payload") FROM stdin;
SO0015	{"CustId": "2", "OrderDetail": [{"Qty": 1, "ProductCode": "eg001"}], "SalesOrderNo": "SO0015"}
SO0017	{"CustId": "2", "OrderDetail": [{"Qty": 12, "ProductCode": "st001"}], "SalesOrderNo": "SO0017"}
\.


--
-- Name: Customer_CustId_seq; Type: SEQUENCE SET; Schema: public; Owner: test
--

SELECT pg_catalog.setval('public."Customer_CustId_seq"', 3, true);


--
-- Name: Price_PriceId_seq; Type: SEQUENCE SET; Schema: public; Owner: test
--

SELECT pg_catalog.setval('public."Price_PriceId_seq"', 4, true);


--
-- Name: sales_order_sequence; Type: SEQUENCE SET; Schema: public; Owner: test
--

SELECT pg_catalog.setval('public.sales_order_sequence', 17, true);


--
-- Name: Customer Customer_pkey; Type: CONSTRAINT; Schema: public; Owner: test
--

ALTER TABLE ONLY public."Customer"
    ADD CONSTRAINT "Customer_pkey" PRIMARY KEY ("CustId");


--
-- Name: Price Price_pkey; Type: CONSTRAINT; Schema: public; Owner: test
--

ALTER TABLE ONLY public."Price"
    ADD CONSTRAINT "Price_pkey" PRIMARY KEY ("PriceId");


--
-- Name: Product Product_pkey; Type: CONSTRAINT; Schema: public; Owner: test
--

ALTER TABLE ONLY public."Product"
    ADD CONSTRAINT "Product_pkey" PRIMARY KEY ("ProductCode");


--
-- Name: SalesOrderInterface SalesOrderInterface_pkey; Type: CONSTRAINT; Schema: public; Owner: test
--

ALTER TABLE ONLY public."SalesOrderInterface"
    ADD CONSTRAINT "SalesOrderInterface_pkey" PRIMARY KEY ("SalesOrderNo");


--
-- Name: SalesOrder SalesOrder_pkey; Type: CONSTRAINT; Schema: public; Owner: test
--

ALTER TABLE ONLY public."SalesOrder"
    ADD CONSTRAINT "SalesOrder_pkey" PRIMARY KEY ("SalesOrderNo");


--
-- Name: SalesOrder sales_order_insert_trigger; Type: TRIGGER; Schema: public; Owner: test
--

CREATE TRIGGER sales_order_insert_trigger AFTER INSERT ON public."SalesOrder" FOR EACH ROW EXECUTE FUNCTION public.insert_sales_order_interface();


--
-- Name: SalesOrder set_sales_order_no; Type: TRIGGER; Schema: public; Owner: test
--

CREATE TRIGGER set_sales_order_no BEFORE INSERT ON public."SalesOrder" FOR EACH ROW EXECUTE FUNCTION public.generate_sales_order_no();


--
-- PostgreSQL database dump complete
--

