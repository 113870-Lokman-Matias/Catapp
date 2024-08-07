import React, { useContext } from "react";
import { Context } from "../../common/Context";

import "./Footer.css";

import Facebook from "../../assets/images/facebook.png";
import Instagram from "../../assets/images/instagram.png";

let today = new Date();
let year = today.getFullYear();

function Footer() {
  const { direccionAuto } = useContext(Context);
  const { urlDireccionAuto } = useContext(Context);

  const { telefonoEmpresa } = useContext(Context);

  const { facebook } = useContext(Context);
  const { urlFacebook } = useContext(Context);

  const { instagram } = useContext(Context);
  const { urlInstagram } = useContext(Context);

  return (
    <footer className="footer">
      <div className="footer-content">
        <div className="footer-content-section-global">
          <div className="footer-content-section">
            <div className="footer-content-section">
              {((facebook && facebook !== "") ||
                (instagram && instagram !== "")) && (
                <div className="social-media">
                  {facebook && facebook !== "" && (
                    <div className="footer-content-section-left-social">
                      <a
                        href={urlFacebook ? urlFacebook : "#"}
                        target={urlFacebook ? "_blank" : ""}
                        rel={urlFacebook ? "noopener noreferrer" : ""}
                      >
                        <img
                          src={Facebook}
                          alt="Facebook"
                          className="social-photo"
                        />
                      </a>
                      <p className="social-title">
                        <a
                          className="social-title"
                          href={urlFacebook ? urlFacebook : "#"}
                          target={urlFacebook ? "_blank" : ""}
                          rel={urlFacebook ? "noopener noreferrer" : ""}
                        >
                          {facebook}
                        </a>
                      </p>
                    </div>
                  )}

                  {instagram && instagram !== "" && (
                    <div className="footer-content-section-left-social">
                      <a
                        className="social-title"
                        href={urlInstagram ? urlInstagram : "#"}
                        target={urlInstagram ? "_blank" : ""}
                        rel={urlInstagram ? "noopener noreferrer" : ""}
                      >
                        <img
                          src={Instagram}
                          alt="Instagram"
                          className="social-photo"
                        />
                      </a>
                      <p className="social-title">
                        <a
                          className="social-title"
                          href={urlInstagram ? urlInstagram : "#"}
                          target={urlInstagram ? "_blank" : ""}
                          rel={urlInstagram ? "noopener noreferrer" : ""}
                        >
                          {instagram}
                        </a>
                      </p>
                    </div>
                  )}
                </div>
              )}

              {((direccionAuto && direccionAuto !== "") ||
                (telefonoEmpresa && telefonoEmpresa !== "")) && (
                <div className="footer-nores">
                  <p className="info-text">
                    {direccionAuto && direccionAuto !== "" && (
                      <a
                        className="address"
                        href={urlDireccionAuto ? urlDireccionAuto : "#"}
                        target={urlDireccionAuto ? "_blank" : ""}
                        rel={urlDireccionAuto ? "noopener noreferrer" : ""}
                      >
                        {direccionAuto}
                      </a>
                    )}

                    {direccionAuto &&
                      direccionAuto !== "" &&
                      telefonoEmpresa &&
                      telefonoEmpresa !== "" && <> - </>}

                    {telefonoEmpresa && telefonoEmpresa !== "" && (
                      <a className="address" href={`tel:${telefonoEmpresa}`}>
                        {telefonoEmpresa}
                      </a>
                    )}
                  </p>
                </div>
              )}

              <div className="footer-res">
                <p className="info-text">
                  <a
                    className="address"
                    href="https://www.google.com/maps/place/C%C3%B3rdoba/@-31.3994267,-64.2767842,12z/data=!3m1!4b1!4m6!3m5!1s0x9432985f478f5b69:0xb0a24f9a5366b092!8m2!3d-31.4200833!4d-64.1887761!16zL20vMDFrMDNy?entry=ttu"
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    Córdoba, Argentina
                  </a>
                </p>
                <p className="info-text">
                  <a className="address" href="tel:03517476389">
                    (0351) 747 6389
                  </a>
                </p>
              </div>
              <p className="footer-text">
                Copyright <i className="footer-href">Catapp</i> | © {year} Todos
                los derechos reservados.
              </p>
            </div>
          </div>
        </div>
      </div>
    </footer>
  );
}

export default Footer;
