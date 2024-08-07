--
-- PostgreSQL database dump
--

-- Dumped from database version 15.3
-- Dumped by pg_dump version 15.3

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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: categorias; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categorias (
    id_categoria integer NOT NULL,
    nombre text NOT NULL,
    id_imagen text NOT NULL,
    url_imagen text NOT NULL,
    ocultar boolean DEFAULT false NOT NULL
);


ALTER TABLE public.categorias OWNER TO postgres;

--
-- Name: categorias_id_categoria_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.categorias_id_categoria_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.categorias_id_categoria_seq OWNER TO postgres;

--
-- Name: categorias_id_categoria_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.categorias_id_categoria_seq OWNED BY public.categorias.id_categoria;


--
-- Name: clientes; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.clientes (
    id_cliente integer NOT NULL,
    nombre_completo text NOT NULL,
    dni bigint NOT NULL,
    telefono bigint NOT NULL,
    direccion text,
    entre_calles text
);


ALTER TABLE public.clientes OWNER TO postgres;

--
-- Name: clientes_id_cliente_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.clientes_id_cliente_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.clientes_id_cliente_seq OWNER TO postgres;

--
-- Name: clientes_id_cliente_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.clientes_id_cliente_seq OWNED BY public.clientes.id_cliente;


--
-- Name: configuraciones; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.configuraciones (
    id_configuracion integer NOT NULL,
    direccion text,
    url_direccion text,
    horarios text,
    cbu text,
    alias text,
    whatsapp text DEFAULT 0 NOT NULL,
    telefono text,
    facebook text,
    url_facebook text,
    instagram text,
    url_instagram text,
    monto_mayorista real DEFAULT 0 NOT NULL,
    url_logo text,
    codigo text
);


ALTER TABLE public.configuraciones OWNER TO postgres;

--
-- Name: configuraciones_id_configuracion_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.configuraciones_id_configuracion_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.configuraciones_id_configuracion_seq OWNER TO postgres;

--
-- Name: configuraciones_id_configuracion_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.configuraciones_id_configuracion_seq OWNED BY public.configuraciones.id_configuracion;


--
-- Name: cotizaciones; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.cotizaciones (
    id_cotizacion integer NOT NULL,
    precio real DEFAULT 0 NOT NULL,
    fecha_modificacion timestamp with time zone NOT NULL,
    ultimo_modificador text NOT NULL
);


ALTER TABLE public.cotizaciones OWNER TO postgres;

--
-- Name: cotizaciones_id_cotizacion_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.cotizaciones_id_cotizacion_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.cotizaciones_id_cotizacion_seq OWNER TO postgres;

--
-- Name: cotizaciones_id_cotizacion_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.cotizaciones_id_cotizacion_seq OWNED BY public.cotizaciones.id_cotizacion;


--
-- Name: detalle_pedidos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.detalle_pedidos (
    id_detalle_pedidos integer NOT NULL,
    id_producto integer NOT NULL,
    cantidad integer NOT NULL,
    aclaracion text,
    precio_unitario real NOT NULL,
    id_pedido uuid NOT NULL
);


ALTER TABLE public.detalle_pedidos OWNER TO postgres;

--
-- Name: detalle_pedidos_id_detalle_pedidos_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.detalle_pedidos_id_detalle_pedidos_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.detalle_pedidos_id_detalle_pedidos_seq OWNER TO postgres;

--
-- Name: detalle_pedidos_id_detalle_pedidos_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.detalle_pedidos_id_detalle_pedidos_seq OWNED BY public.detalle_pedidos.id_detalle_pedidos;


--
-- Name: detalles_stock; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.detalles_stock (
    id_detalles_stock integer NOT NULL,
    accion text NOT NULL,
    cantidad integer NOT NULL,
    motivo text NOT NULL,
    fecha timestamp with time zone NOT NULL,
    modificador text NOT NULL,
    id_producto integer NOT NULL
);


ALTER TABLE public.detalles_stock OWNER TO postgres;

--
-- Name: detalles_stock_id_detalles_stock_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.detalles_stock_id_detalles_stock_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.detalles_stock_id_detalles_stock_seq OWNER TO postgres;

--
-- Name: detalles_stock_id_detalles_stock_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.detalles_stock_id_detalles_stock_seq OWNED BY public.detalles_stock.id_detalles_stock;


--
-- Name: divisas; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.divisas (
    id_divisa integer NOT NULL,
    nombre text NOT NULL
);


ALTER TABLE public.divisas OWNER TO postgres;

--
-- Name: divisas_id_divisa_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.divisas_id_divisa_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.divisas_id_divisa_seq OWNER TO postgres;

--
-- Name: divisas_id_divisa_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.divisas_id_divisa_seq OWNED BY public.divisas.id_divisa;


--
-- Name: envios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.envios (
    id_envio integer NOT NULL,
    habilitado boolean DEFAULT false NOT NULL,
    costo real DEFAULT 0 NOT NULL,
    fecha_modificacion timestamp with time zone NOT NULL,
    ultimo_modificador text NOT NULL,
    nombre text NOT NULL,
    disponibilidad_catalogo integer NOT NULL,
    aclaracion text
);


ALTER TABLE public.envios OWNER TO postgres;

--
-- Name: envios_id_envio_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.envios_id_envio_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.envios_id_envio_seq OWNER TO postgres;

--
-- Name: envios_id_envio_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.envios_id_envio_seq OWNED BY public.envios.id_envio;


--
-- Name: metodos_pago; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.metodos_pago (
    id_metodo_pago integer NOT NULL,
    nombre text NOT NULL,
    habilitado boolean NOT NULL,
    disponibilidad integer NOT NULL,
    disponibilidad_catalogo integer NOT NULL
);


ALTER TABLE public.metodos_pago OWNER TO postgres;

--
-- Name: metodos_pago_id_metodo_pago_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.metodos_pago_id_metodo_pago_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.metodos_pago_id_metodo_pago_seq OWNER TO postgres;

--
-- Name: metodos_pago_id_metodo_pago_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.metodos_pago_id_metodo_pago_seq OWNED BY public.metodos_pago.id_metodo_pago;


--
-- Name: pedidos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.pedidos (
    id_pedido uuid NOT NULL,
    costo_envio real NOT NULL,
    fecha timestamp with time zone NOT NULL,
    verificado boolean DEFAULT false NOT NULL,
    direccion text,
    entre_calles text,
    payment_id text,
    id_tipo_pedido integer NOT NULL,
    id_vendedor integer,
    id_metodo_pago integer NOT NULL,
    id_cliente integer NOT NULL,
    id_metodo_entrega integer NOT NULL
);


ALTER TABLE public.pedidos OWNER TO postgres;

--
-- Name: productos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.productos (
    id_producto integer NOT NULL,
    nombre text NOT NULL,
    descripcion text NOT NULL,
    precio real NOT NULL,
    porcentaje_minorista real NOT NULL,
    porcentaje_mayorista real NOT NULL,
    precio_minorista real NOT NULL,
    precio_mayorista real NOT NULL,
    stock integer DEFAULT 0 NOT NULL,
    id_categoria integer NOT NULL,
    id_imagen text NOT NULL,
    url_imagen text NOT NULL,
    ocultar boolean DEFAULT false NOT NULL,
    en_promocion boolean DEFAULT false NOT NULL,
    en_destacado boolean DEFAULT false NOT NULL,
    id_divisa integer NOT NULL,
    stock_transitorio integer NOT NULL,
    id_subcategoria integer
);


ALTER TABLE public.productos OWNER TO postgres;

--
-- Name: productos_id_producto_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.productos_id_producto_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.productos_id_producto_seq OWNER TO postgres;

--
-- Name: productos_id_producto_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.productos_id_producto_seq OWNED BY public.productos.id_producto;


--
-- Name: roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.roles (
    id_rol integer NOT NULL,
    nombre text NOT NULL
);


ALTER TABLE public.roles OWNER TO postgres;

--
-- Name: roles_id_rol_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.roles_id_rol_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.roles_id_rol_seq OWNER TO postgres;

--
-- Name: roles_id_rol_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.roles_id_rol_seq OWNED BY public.roles.id_rol;


--
-- Name: subcategorias; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subcategorias (
    id_subcategoria integer NOT NULL,
    nombre text NOT NULL,
    ocultar boolean DEFAULT false NOT NULL,
    id_categoria integer NOT NULL
);


ALTER TABLE public.subcategorias OWNER TO postgres;

--
-- Name: subcategorias_id_subcategoria_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.subcategorias_id_subcategoria_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.subcategorias_id_subcategoria_seq OWNER TO postgres;

--
-- Name: subcategorias_id_subcategoria_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.subcategorias_id_subcategoria_seq OWNED BY public.subcategorias.id_subcategoria;


--
-- Name: tipos_pedido; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tipos_pedido (
    id_tipo_pedido integer NOT NULL,
    nombre text NOT NULL
);


ALTER TABLE public.tipos_pedido OWNER TO postgres;

--
-- Name: tipos_pedido_id_tipo_pedido_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tipos_pedido_id_tipo_pedido_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tipos_pedido_id_tipo_pedido_seq OWNER TO postgres;

--
-- Name: tipos_pedido_id_tipo_pedido_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tipos_pedido_id_tipo_pedido_seq OWNED BY public.tipos_pedido.id_tipo_pedido;


--
-- Name: usuarios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.usuarios (
    id_usuario integer NOT NULL,
    nombre text NOT NULL,
    username text NOT NULL,
    password text NOT NULL,
    activo boolean NOT NULL,
    email text NOT NULL,
    codigo_verificacion integer NOT NULL,
    id_rol integer NOT NULL
);


ALTER TABLE public.usuarios OWNER TO postgres;

--
-- Name: usuarios_id_usuario_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.usuarios_id_usuario_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.usuarios_id_usuario_seq OWNER TO postgres;

--
-- Name: usuarios_id_usuario_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.usuarios_id_usuario_seq OWNED BY public.usuarios.id_usuario;


--
-- Name: categorias id_categoria; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categorias ALTER COLUMN id_categoria SET DEFAULT nextval('public.categorias_id_categoria_seq'::regclass);


--
-- Name: clientes id_cliente; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clientes ALTER COLUMN id_cliente SET DEFAULT nextval('public.clientes_id_cliente_seq'::regclass);


--
-- Name: configuraciones id_configuracion; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.configuraciones ALTER COLUMN id_configuracion SET DEFAULT nextval('public.configuraciones_id_configuracion_seq'::regclass);


--
-- Name: cotizaciones id_cotizacion; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cotizaciones ALTER COLUMN id_cotizacion SET DEFAULT nextval('public.cotizaciones_id_cotizacion_seq'::regclass);


--
-- Name: detalle_pedidos id_detalle_pedidos; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalle_pedidos ALTER COLUMN id_detalle_pedidos SET DEFAULT nextval('public.detalle_pedidos_id_detalle_pedidos_seq'::regclass);


--
-- Name: detalles_stock id_detalles_stock; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalles_stock ALTER COLUMN id_detalles_stock SET DEFAULT nextval('public.detalles_stock_id_detalles_stock_seq'::regclass);


--
-- Name: divisas id_divisa; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.divisas ALTER COLUMN id_divisa SET DEFAULT nextval('public.divisas_id_divisa_seq'::regclass);


--
-- Name: envios id_envio; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.envios ALTER COLUMN id_envio SET DEFAULT nextval('public.envios_id_envio_seq'::regclass);


--
-- Name: metodos_pago id_metodo_pago; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.metodos_pago ALTER COLUMN id_metodo_pago SET DEFAULT nextval('public.metodos_pago_id_metodo_pago_seq'::regclass);


--
-- Name: productos id_producto; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.productos ALTER COLUMN id_producto SET DEFAULT nextval('public.productos_id_producto_seq'::regclass);


--
-- Name: roles id_rol; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles ALTER COLUMN id_rol SET DEFAULT nextval('public.roles_id_rol_seq'::regclass);


--
-- Name: subcategorias id_subcategoria; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subcategorias ALTER COLUMN id_subcategoria SET DEFAULT nextval('public.subcategorias_id_subcategoria_seq'::regclass);


--
-- Name: tipos_pedido id_tipo_pedido; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tipos_pedido ALTER COLUMN id_tipo_pedido SET DEFAULT nextval('public.tipos_pedido_id_tipo_pedido_seq'::regclass);


--
-- Name: usuarios id_usuario; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios ALTER COLUMN id_usuario SET DEFAULT nextval('public.usuarios_id_usuario_seq'::regclass);


--
-- Data for Name: categorias; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.categorias (id_categoria, nombre, id_imagen, url_imagen, ocultar) FROM stdin;
\.


--
-- Data for Name: clientes; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.clientes (id_cliente, nombre_completo, dni, telefono, direccion, entre_calles) FROM stdin;
\.


--
-- Data for Name: configuraciones; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.configuraciones (id_configuracion, direccion, url_direccion, horarios, cbu, alias, whatsapp, telefono, facebook, url_facebook, instagram, url_instagram, monto_mayorista, url_logo, codigo) FROM stdin;
1	\N	\N	\N	\N	\N	0	\N	\N	\N	\N	\N	0	\N	\N
\.


--
-- Data for Name: cotizaciones; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.cotizaciones (id_cotizacion, precio, fecha_modificacion, ultimo_modificador) FROM stdin;
2	0	2024-03-25 14:00:00-03	SuperAdmin
\.


--
-- Data for Name: detalle_pedidos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.detalle_pedidos (id_detalle_pedidos, id_producto, cantidad, aclaracion, precio_unitario, id_pedido) FROM stdin;
\.


--
-- Data for Name: detalles_stock; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.detalles_stock (id_detalles_stock, accion, cantidad, motivo, fecha, modificador, id_producto) FROM stdin;
\.


--
-- Data for Name: divisas; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.divisas (id_divisa, nombre) FROM stdin;
1	Dólar
2	Peso
\.


--
-- Data for Name: envios; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.envios (id_envio, habilitado, costo, fecha_modificacion, ultimo_modificador, nombre, disponibilidad_catalogo, aclaracion) FROM stdin;
1	t	0	2024-03-25 14:00:00-03	SuperAdmin	Retiro por el local	3	\N
2	t	0	2024-03-25 14:00:00-03	SuperAdmin	Envío a domicilio	1	Dentro anillo de circunvalación
3	t	0	2024-03-25 14:00:00-03	SuperAdmin	Envío a domicilio	1	Fuera anillo de circunvalación
\.


--
-- Data for Name: metodos_pago; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.metodos_pago (id_metodo_pago, nombre, habilitado, disponibilidad, disponibilidad_catalogo) FROM stdin;
1	Efectivo	f	1	3
2	Transferencia	f	1	1
3	Tarjeta de débito	f	1	1
4	Tarjeta de crédito	f	1	1
5	Mercado Pago	f	3	1
\.


--
-- Data for Name: pedidos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.pedidos (id_pedido, costo_envio, fecha, verificado, direccion, entre_calles, payment_id, id_tipo_pedido, id_vendedor, id_metodo_pago, id_cliente, id_metodo_entrega) FROM stdin;
\.


--
-- Data for Name: productos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.productos (id_producto, nombre, descripcion, precio, porcentaje_minorista, porcentaje_mayorista, precio_minorista, precio_mayorista, stock, id_categoria, id_imagen, url_imagen, ocultar, en_promocion, en_destacado, id_divisa, stock_transitorio, id_subcategoria) FROM stdin;
\.


--
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.roles (id_rol, nombre) FROM stdin;
1	SuperAdmin
2	Admin
3	Gerente
4	Supervisor
5	Vendedor
6	Predeterminado
\.


--
-- Data for Name: subcategorias; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.subcategorias (id_subcategoria, nombre, ocultar, id_categoria) FROM stdin;
\.


--
-- Data for Name: tipos_pedido; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tipos_pedido (id_tipo_pedido, nombre) FROM stdin;
1	Minorista
2	Mayorista
\.


--
-- Data for Name: usuarios; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.usuarios (id_usuario, nombre, username, password, activo, email, codigo_verificacion, id_rol) FROM stdin;
8	Super Admin	superadmin	$2a$10$O240kzrAHcO11NbL7NZyKuv7TtpfKVu3WQ3KJys3mEDvamjXbM6eq	t	matias.lokman@gmail.com	0	1
9	Admin	admin	$2a$10$O240kzrAHcO11NbL7NZyKuv7TtpfKVu3WQ3KJys3mEDvamjXbM6eq	t	mati.lokman123@gmail.com	0	2
\.


--
-- Name: categorias_id_categoria_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.categorias_id_categoria_seq', 1, false);


--
-- Name: clientes_id_cliente_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.clientes_id_cliente_seq', 1, false);


--
-- Name: configuraciones_id_configuracion_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.configuraciones_id_configuracion_seq', 1, true);


--
-- Name: cotizaciones_id_cotizacion_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.cotizaciones_id_cotizacion_seq', 2, true);


--
-- Name: detalle_pedidos_id_detalle_pedidos_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.detalle_pedidos_id_detalle_pedidos_seq', 1, false);


--
-- Name: detalles_stock_id_detalles_stock_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.detalles_stock_id_detalles_stock_seq', 1, false);


--
-- Name: divisas_id_divisa_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.divisas_id_divisa_seq', 2, true);


--
-- Name: envios_id_envio_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.envios_id_envio_seq', 3, true);


--
-- Name: metodos_pago_id_metodo_pago_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.metodos_pago_id_metodo_pago_seq', 5, true);


--
-- Name: productos_id_producto_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.productos_id_producto_seq', 1, false);


--
-- Name: roles_id_rol_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.roles_id_rol_seq', 6, true);


--
-- Name: subcategorias_id_subcategoria_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.subcategorias_id_subcategoria_seq', 1, false);


--
-- Name: tipos_pedido_id_tipo_pedido_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tipos_pedido_id_tipo_pedido_seq', 2, true);


--
-- Name: usuarios_id_usuario_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.usuarios_id_usuario_seq', 10, true);


--
-- Name: categorias categorias_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categorias
    ADD CONSTRAINT categorias_pkey PRIMARY KEY (id_categoria);


--
-- Name: clientes clientes_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.clientes
    ADD CONSTRAINT clientes_pkey PRIMARY KEY (id_cliente);


--
-- Name: configuraciones configuraciones_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.configuraciones
    ADD CONSTRAINT configuraciones_pkey PRIMARY KEY (id_configuracion);


--
-- Name: cotizaciones cotizaciones_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.cotizaciones
    ADD CONSTRAINT cotizaciones_pkey PRIMARY KEY (id_cotizacion);


--
-- Name: detalle_pedidos detalle_pedidos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalle_pedidos
    ADD CONSTRAINT detalle_pedidos_pkey PRIMARY KEY (id_detalle_pedidos);


--
-- Name: detalles_stock detalles_stock_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalles_stock
    ADD CONSTRAINT detalles_stock_pkey PRIMARY KEY (id_detalles_stock);


--
-- Name: divisas divisas_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.divisas
    ADD CONSTRAINT divisas_pkey PRIMARY KEY (id_divisa);


--
-- Name: envios envios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.envios
    ADD CONSTRAINT envios_pkey PRIMARY KEY (id_envio);


--
-- Name: metodos_pago metodos_pago_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.metodos_pago
    ADD CONSTRAINT metodos_pago_pkey PRIMARY KEY (id_metodo_pago);


--
-- Name: pedidos pedidos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT pedidos_pkey PRIMARY KEY (id_pedido);


--
-- Name: productos productos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.productos
    ADD CONSTRAINT productos_pkey PRIMARY KEY (id_producto);


--
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (id_rol);


--
-- Name: subcategorias subcategorias_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subcategorias
    ADD CONSTRAINT subcategorias_pkey PRIMARY KEY (id_subcategoria);


--
-- Name: tipos_pedido tipos_pedido_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tipos_pedido
    ADD CONSTRAINT tipos_pedido_pkey PRIMARY KEY (id_tipo_pedido);


--
-- Name: usuarios usuarios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_pkey PRIMARY KEY (id_usuario);


--
-- Name: fki_fk_categoria; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_categoria ON public.productos USING btree (id_categoria);


--
-- Name: fki_fk_cliente; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_cliente ON public.pedidos USING btree (id_cliente);


--
-- Name: fki_fk_divisa; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_divisa ON public.productos USING btree (id_divisa);


--
-- Name: fki_fk_metodo_entrega; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_metodo_entrega ON public.pedidos USING btree (id_metodo_entrega);


--
-- Name: fki_fk_metodo_pago; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_metodo_pago ON public.pedidos USING btree (id_metodo_pago);


--
-- Name: fki_fk_pedido; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_pedido ON public.detalle_pedidos USING btree (id_pedido);


--
-- Name: fki_fk_producto; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_producto ON public.detalle_pedidos USING btree (id_producto);


--
-- Name: fki_fk_rol; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_rol ON public.usuarios USING btree (id_rol);


--
-- Name: fki_fk_subcategoria; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_subcategoria ON public.productos USING btree (id_subcategoria);


--
-- Name: fki_fk_tipo_pedido; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tipo_pedido ON public.pedidos USING btree (id_tipo_pedido);


--
-- Name: fki_fk_vendedor; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_vendedor ON public.pedidos USING btree (id_vendedor);


--
-- Name: productos fk_categoria; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.productos
    ADD CONSTRAINT fk_categoria FOREIGN KEY (id_categoria) REFERENCES public.categorias(id_categoria) NOT VALID;


--
-- Name: subcategorias fk_categoria; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subcategorias
    ADD CONSTRAINT fk_categoria FOREIGN KEY (id_categoria) REFERENCES public.categorias(id_categoria) NOT VALID;


--
-- Name: pedidos fk_cliente; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT fk_cliente FOREIGN KEY (id_cliente) REFERENCES public.clientes(id_cliente) NOT VALID;


--
-- Name: productos fk_divisa; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.productos
    ADD CONSTRAINT fk_divisa FOREIGN KEY (id_divisa) REFERENCES public.divisas(id_divisa) NOT VALID;


--
-- Name: pedidos fk_metodo_entrega; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT fk_metodo_entrega FOREIGN KEY (id_metodo_entrega) REFERENCES public.envios(id_envio) NOT VALID;


--
-- Name: pedidos fk_metodo_pago; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT fk_metodo_pago FOREIGN KEY (id_metodo_pago) REFERENCES public.metodos_pago(id_metodo_pago) NOT VALID;


--
-- Name: detalle_pedidos fk_pedido; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalle_pedidos
    ADD CONSTRAINT fk_pedido FOREIGN KEY (id_pedido) REFERENCES public.pedidos(id_pedido) NOT VALID;


--
-- Name: detalle_pedidos fk_producto; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalle_pedidos
    ADD CONSTRAINT fk_producto FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto) NOT VALID;


--
-- Name: detalles_stock fk_producto; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.detalles_stock
    ADD CONSTRAINT fk_producto FOREIGN KEY (id_producto) REFERENCES public.productos(id_producto) NOT VALID;


--
-- Name: usuarios fk_rol; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT fk_rol FOREIGN KEY (id_rol) REFERENCES public.roles(id_rol) NOT VALID;


--
-- Name: productos fk_subcategoria; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.productos
    ADD CONSTRAINT fk_subcategoria FOREIGN KEY (id_subcategoria) REFERENCES public.subcategorias(id_subcategoria) NOT VALID;


--
-- Name: pedidos fk_tipo_pedido; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT fk_tipo_pedido FOREIGN KEY (id_tipo_pedido) REFERENCES public.tipos_pedido(id_tipo_pedido) NOT VALID;


--
-- Name: pedidos fk_vendedor; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pedidos
    ADD CONSTRAINT fk_vendedor FOREIGN KEY (id_vendedor) REFERENCES public.usuarios(id_usuario) NOT VALID;


--
-- PostgreSQL database dump complete
--

