import "./styles/global.css";
import "./styles/global2.css";

import { BrowserRouter, Route, Routes } from "react-router-dom";

import ScrollToTop from "./components/ScrollToTop/ScrollToTop";
import Header from "./components/Header/Header";
import NotFound from "./pages/NotFound/NotFound";
import WhatsApp from "./components/Whatsapp/Whatsapp";
import ScrollToTopBtn from "./components/ScrollToTopBtn/ScrollToTopBtn";
import Footer from "./components/Footer/Footer";

import Catalogo from "./pages/frontOffice/Catalogo/Catalogo";

import Login from "./pages/frontOffice/Login/Login";
import CreateUser from "./pages/frontOffice/CreateUser/CreateUser";
import ResetPassword from "./pages/frontOffice/ResetPassword/ResetPassword";

import AdminPanel from "./pages/backOffice/AdminPanel/AdminPanel";
import UserManager from "./pages/backOffice/managers/UserManager/UserManager";
import CategoryManager from "./pages/backOffice/managers/CategoryManager/CategoryManager";
import DollarManager from "./pages/backOffice/managers/DollarManager/DollarManager";
import ProductManager from "./pages/backOffice/managers/ProductManager/ProductManager";

import { ProtectedRoute } from "./components/ProtectedRoute/ProtectedRoute";

function App() {
  return (
    <BrowserRouter>
      <ScrollToTop />
      <Header />
      <Routes>
        <Route index element={<Catalogo />} />
        <Route path="/" element={<Catalogo />} />
        <Route path="catalogo-minorista" element={<Catalogo />} />
        <Route path="catalogo-mayorista" element={<Catalogo />} />

        <Route path="login" element={<Login />} />
        <Route path="create-user" element={<CreateUser />} />
        <Route path="reset-password" element={<ResetPassword />} />

        <Route element={<ProtectedRoute />}>
          <Route path="panel-de-administrador" element={<AdminPanel />} />
          <Route path="administrar-usuarios" element={<UserManager />} />
          <Route path="administrar-gerentes" element={<UserManager />} />
          <Route path="administrar-supervisores" element={<UserManager />} />
          <Route path="administrar-vendedores" element={<UserManager />} />
          <Route path="administrar-categorias" element={<CategoryManager />} />
          <Route path="administrar-cotizacion" element={<DollarManager />} />
          <Route path="administrar-productos" element={<ProductManager />} />
        </Route>

        <Route path="*" element={<NotFound />} />
      </Routes>
      <Footer />
      <WhatsApp />
      <ScrollToTopBtn />
    </BrowserRouter>
  );
}

export default App;
