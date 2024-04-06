import React from "react";

import "./Footer.css";

import Facebook from "../../assets/images/facebook.png";
import Instagram from "../../assets/images/instagram.png";

let today = new Date();
let year = today.getFullYear();

function Footer() {
  return (
    <footer className="footer">
      <div className="footer-content">
        <div className="footer-content-section-global">
          <div className="footer-content-section">
            <div className="footer-content-section">
              <div className="social-media">
                <div className="footer-content-section-left-social">
                  <a
                    href="https://www.facebook.com/"
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    <img
                      src={Facebook}
                      alt="Facebook"
                      className="social-photo"
                    />
                  </a>
                  <p className="social-title facebook">
                    <a
                      className="social-title"
                      href="https://www.facebook.com/"
                      target="_blank"
                      rel="noopener noreferrer"
                    >
                      Catapp
                    </a>
                  </p>
                </div>
                <div className="footer-content-section-left-social">
                  <a
                    className="social-title"
                    href="https://www.instagram.com/"
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    <img
                      src={Instagram}
                      alt="Instagram"
                      className="social-photo"
                    />
                  </a>
                  <p className="social-title instagram">
                    <a
                      className="social-title"
                      href="https://www.instagram.com/"
                      target="_blank"
                      rel="noopener noreferrer"
                    >
                      Catapp
                    </a>
                  </p>
                </div>
              </div>
              <div className="footer-nores">
                <p className="info-text">
                  <a
                    className="address"
                    href="https://www.google.com/maps/place/C%C3%B3rdoba/@-31.3994267,-64.2767842,12z/data=!3m1!4b1!4m6!3m5!1s0x9432985f478f5b69:0xb0a24f9a5366b092!8m2!3d-31.4200833!4d-64.1887761!16zL20vMDFrMDNy?entry=ttu"
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    Córdoba, Argentina
                  </a>{" "}
                  -{" "}
                  <a className="address" href="tel:03517476389">
                    (0351) 747 6389
                  </a>
                </p>
              </div>
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
