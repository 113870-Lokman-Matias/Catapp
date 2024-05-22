import React from "react";
import { Helmet } from "react-helmet";
import { Link } from "react-router-dom";
import { ReactComponent as Back } from "../../../assets/svgs/back.svg";

import "./TermsConditions.css";

function TermsConditions() {
  //#region Return
  return (
    <div>
      <Helmet>
        <title>Catapp | Términos y condiciones</title>
      </Helmet>
      <section className="general-container">
        <div className="general-content">
          <div className="title-header mb">
            <Link
              to="/panel"
              className="btn btn-info btn-back"
            >
              <div className="btn-back-content">
                <Back className="back" />
                <p className="p-back">Regresar</p>
              </div>
            </Link>

            <h2 className="title title-general">
              Términos y condiciones
            </h2>
          </div>
          <div className="terms-content">
            <p>
              Bienvenido a Catapp proporcionado por Matías Lokman. Nos complace
              ofrecerle acceso al Servicio (como se define más abajo), sujeto a
              estos términos y condiciones (los "Términos de Servicio") y a la
              Política de Privacidad correspondiente de Catapp. Al acceder y
              utilizar el Servicio, usted expresa su consentimiento, acuerdo y
              entendimiento de los Términos de Servicio y la Política de
              Privacidad. Si no está de acuerdo con los Términos de Servicio o
              la Política de Privacidad, no utilice el Servicio. Si utiliza el
              servicio está aceptando las modalidades operativas en vigencia
              descriptas más adelante, las declara conocer y aceptar, las que se
              habiliten en el futuro y en los términos y condiciones que a
              continuación se detallan:
            </p>

            <div className="one-term">
              <h3 className="subtitle">Operaciones habilitadas:</h3>
              <p>
                Las operaciones habilitadas son aquellas que estarán disponibles
                para los clientes, quienes deberán cumplir los requisitos que se
                encuentren vigentes en su momento para operar el Servicio. Las
                mismas podrán ser ampliadas o restringidas por el proveedor,
                comunicándolo previamente con una antelación no menor a 60 días,
                y comprenden entre otras, sin que pueda entenderse taxativamente
                las que se indican a continuación:
              </p>
            </div>

            <div className="one-term">
              <h3 className="subtitle">Transacciones:</h3>
              <p>
                En ningún caso debe entenderse que la solicitud de un producto o
                servicio implica obligación alguna para el Acceso y uso del
                Servicio. Para operar el Servicio se requerirá siempre que se
                trate de clientes de Catapp, quienes podrán acceder mediante
                cualquier dispositivo con conexión a la Red Internet. El cliente
                deberá proporcionar el número de documento de identidad y la
                clave personal, que será provista por la aplicación como
                requisito previo a la primera operación, en la forma que le sea
                requerida. La clave personal y todo o cualquier otro mecanismo
                adicional de autenticación personal provisto por el Banco tiene
                el carácter de secreto e intransferible, y por lo tanto asumo
                las consecuencias de su divulgación a terceros, liberando a
                Matías Lokman de toda responsabilidad que de ello se derive. En
                ningún caso Catapp requerirá que le suministre la totalidad de
                los datos, ni enviará mail requiriendo información personal
                alguna.
              </p>
            </div>

            <div className="one-term">
              <h3 className="subtitle">Costo del Servicio:</h3>
              <p>
                Matías Lokman podrá cobrar comisiones por el mantenimiento y/o
                uso de este Servicio o los que en el futuro implemente,
                entendiéndose facultado expresamente para efectuar los
                correspondientes débitos en mis cuentas, aún en descubierto, por
                lo que presto para ello mi expresa conformidad. En caso de
                cualquier modificación a la presente previsión, lo comunicará
                con al menos 60 días de antelación.
              </p>
            </div>

            <div className="one-term">
              <h3 className="subtitle">Vigencia:</h3>
              <p>
                El Usuario podrá dejar sin efecto la relación que surja de la
                presente, en forma inmediata, sin otra responsabilidad que la
                derivada de los gastos originados hasta ese momento. Si el
                cliente incumpliera cualquiera de las obligaciones asumidas en
                su relación contractual con Matías Lokman, o de los presentes
                Términos y Condiciones, el Banco podrá decretar la caducidad del
                presente Servicio en forma inmediata, sin que ello genere
                derecho a indemnización o compensación alguna. Matías Lokman
                podrá dejar sin efecto la relación que surja de la presente, con
                un preaviso mínimo de 60 días, sin otra responsabilidad.
              </p>
            </div>

            <div className="one-term">
              <h3 className="subtitle">
                Validez de operaciones y notificaciones:
              </h3>
              <p>
                Los registros emitidos por la app serán prueba suficiente de las
                operaciones cursadas por dicho canal. Renuncio expresamente a
                cuestionar la idoneidad o habilidad de ese medio de prueba. A
                los efectos del cumplimiento de disposiciones legales o
                contractuales, se otorga a las notificaciones por este medio el
                mismo alcance de las notificaciones mediante documento escrito.
              </p>
            </div>

            <div className="one-term">
              <h3 className="subtitle">Propiedad intelectual:</h3>
              <p>
                El software en Argentina está protegido por la ley 11.723, que
                regula la propiedad intelectual y los derechos de autor de todos
                aquellos creadores de obras artísticas, literarias y
                científicas.
              </p>
            </div>

            <div className="one-term">
              <h3 className="subtitle">Privacidad de la información:</h3>
              <p>
                Para utilizar los Servicios ofrecidos por Catapp, los Usuarios
                deberán facilitar determinados datos de carácter personal. Su
                información personal se procesa y almacena en servidores o
                medios magnéticos que mantienen altos estándares de seguridad y
                protección tanto física como tecnológica. Para mayor información
                sobre la privacidad de los Datos Personales y casos en los que
                será revelada la información personal, se pueden consultar
                nuestras políticas de privacidad.
              </p>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
  //#endregion
}

export default TermsConditions;
