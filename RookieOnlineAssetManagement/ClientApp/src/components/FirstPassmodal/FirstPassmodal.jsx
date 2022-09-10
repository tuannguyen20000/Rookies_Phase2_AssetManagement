import { useState } from "react";
import axios from "axios";

import Modal from "react-bootstrap/Modal";
import Col from "react-bootstrap/Col";
import Form from "react-bootstrap/Form";
import Row from "react-bootstrap/Row";
import "./FirstPassmodal.css";

function FirstPassmodal(props) {
  const [show, setShow] = useState(true);
  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState([]);

  const handleChange = (event) => {
    setPassword(event.target.value);
  };

  const handleValidation = () => {
    const passwordInputValue = password;

    //for password
    const uppercaseRegExp = /(?=.*?[A-Z])/;
    const lowercaseRegExp = /(?=.*?[a-z])/;
    const digitsRegExp = /(?=.*?[0-9])/;
    const specialCharRegExp = /(?=.*?[#?!@$%^&*-])/;
    const minLengthRegExp = /.{6,}/;
    const uppercasePassword = uppercaseRegExp.test(passwordInputValue);
    const lowercasePassword = lowercaseRegExp.test(passwordInputValue);
    const digitsPassword = digitsRegExp.test(passwordInputValue);
    const specialCharPassword = specialCharRegExp.test(passwordInputValue);
    const minLengthPassword = minLengthRegExp.test(passwordInputValue);

    setPasswordError([]);
    if (
      uppercasePassword &&
      lowercasePassword &&
      digitsPassword &&
      specialCharPassword &&
      minLengthPassword
    ) {
      axios
        .post("/api/users/ChangeFirstPassword", {
          Id: props.id,
          NewPassword: passwordInputValue,
        })
        .then((result) => {});
      setShow(false);
    } else {
      if (!uppercasePassword) {
        setPasswordError((error) => [...error, "At least one Uppercase"]);
      }
      if (!lowercasePassword) {
        setPasswordError((error) => [...error, "At least one Lowercase"]);
      }
      if (!digitsPassword) {
        setPasswordError((error) => [...error, "At least one digit"]);
      }
      if (!specialCharPassword) {
        setPasswordError((error) => [
          ...error,
          "At least one Special Characters",
        ]);
      }
      if (!minLengthPassword) {
        setPasswordError((error) => [
          ...error,
          "At least minumum 6 characters",
        ]);
      }
    }
  };

  return (
    <div className="modal">
      <Modal
        show={show}
        onHide={handleValidation}
        backdrop="static"
        keyboard={false}
      >
        <Modal.Header>
          <Modal.Title>
            <p>Change password</p>
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p style={{ marginBottom: 0 }}>
            This is the first time you logged in.
          </p>
          <p> You have to change your password to continue.</p>
          <Row>
            <Form.Label column lg={4}>
              New password
            </Form.Label>
            <Col>
              <Form.Control
                type="password"
                value={password}
                onChange={handleChange}
              />
            </Col>
          </Row>
          <div className="modal-change-password-error-list">
            <ul>
              {passwordError.map((errors) => (
                <li>{errors}</li>
              ))}
            </ul>
          </div>
          <div className="modal-change-password-button-container">
            <button
              className="btn"
              disabled={!password}
              onClick={handleValidation}
            >
              Save
            </button>
          </div>
        </Modal.Body>
      </Modal>
    </div>
  );
}

export default FirstPassmodal;
