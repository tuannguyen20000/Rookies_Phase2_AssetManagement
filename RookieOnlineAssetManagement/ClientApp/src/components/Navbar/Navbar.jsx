import { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import axios from "axios";

import { Modal, Form, Row, Col } from "react-bootstrap";
import "./Navbar.css";

function Navbar(props) {
  const location = useLocation();
  const [displayMenu, setDisplayMenu] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [id, setId] = useState(null);
  const [showLogout, setShowLogout] = useState(false);
  const [showSuccess, setShowSuccess] = useState(false);
  const [OldPassword, setOldPassword] = useState("");
  const [NewPassword, setNewPassword] = useState("");
  const [ConfirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState([]);

  const renderTitle = (title) => {
    const urlTitle = title.split("/");
    switch (urlTitle[1]) {
      case "":
        return "Home";
      case "users":
        switch (urlTitle[2]) {
          case "add":
            return "Manage User > Add User";
          case "update":
            return "Manage User > Edit User";
          default:
            return "Manage User";
        }
      case "assets":
        switch (urlTitle[2]) {
          case "add":
            return "Manage Asset > Add Asset";
          case "update":
            return "Manage Asset > Edit Asset";
          default:
            return "Manage Asset";
        }
      case "assignments":
        switch (urlTitle[2]) {
          case "add":
            return "Manage Assignment > Add Assignment";
          case "update":
            return "Manage Assignment > Edit Assignment";
          default:
            return "Manage Assignment";
        }
      case "requests":
        return "Request for Returning";
      case "reports":
        return "Report";
      default:
        break;
    }
  };

  const handleClick = (event) => {
    setDisplayMenu((current) => !current);
  };

  const Logout = () => {
    axios.post("/api/users/logout").then((res) => {
      window.location.reload();
    });
  };

  //change password
  const handleClosePassword = () => {
    setOldPassword("");
    setNewPassword("");
    setConfirmPassword("");
    setError([]);
    setShowPassword(false);
  };
  const handleShowPassword = () => setShowPassword(true);

  //modal logout
  const handleCloseLogout = () => setShowLogout(false);
  const handleShowLogout = () => setShowLogout(true);

  //modal success
  const handleCloseSuccess = () => setShowSuccess(false);
  const handleShowSuccess = () => setShowSuccess(true);

  const handleChangeOldPassword = (event) => {
    setOldPassword(event.target.value);
  };

  const handleChangeNewPassword = (event) => {
    setNewPassword(event.target.value);
  };

  const handleChangeConfirmPassword = (event) => {
    setConfirmPassword(event.target.value);
  };

  const isDisabled = () => {
    if (NewPassword !== "" && OldPassword !== "" && ConfirmPassword !== "")
      return false;
    return true;
  };

  useEffect(() => {
    axios.get("/api/users/get-user").then((response) => {
      setId(response.data.id);
    });
  }, []);

  const ChangePassword = () => {
    setError([]);
    const lowercaseRegExp = /(?=.*?[a-z])/;
    const digitsRegExp = /(?=.*?[0-9])/;
    const minLengthRegExp = /.{6,}/;
    const lowercasePassword = lowercaseRegExp.test(NewPassword);
    const digitsPassword = digitsRegExp.test(NewPassword);
    const minLengthPassword = minLengthRegExp.test(NewPassword);
    if (
      lowercasePassword &&
      digitsPassword &&
      minLengthPassword &&
      NewPassword === ConfirmPassword
    ) {
      axios
        .post("/api/users/ChangeNewPassword", {
          Id: id,
          OldPassword: OldPassword,
          NewPassword: NewPassword,
          ConfirmPassword: ConfirmPassword,
        })
        .then((res) => {
          handleClosePassword();
          handleShowSuccess();
        })
        .catch(function (error) {
          setError((error) => [...error, "Password is incorrect!"]);
          return Promise.resolve({ error });
        });
    } else {
      if (!lowercasePassword) {
        setError((error) => [...error, "At least one Lowercase"]);
      }
      if (!digitsPassword) {
        setError((error) => [...error, "At least one digit"]);
      }
      if (!minLengthPassword) {
        setError((error) => [...error, "At least minumum 6 characters"]);
      }
      if (NewPassword !== ConfirmPassword) {
        setError((error) => [...error, "Password not match!"]);
      }
    }
  };

  return (
    <div className="navigationbar">
      <p>{renderTitle(location.pathname)}</p>
      <div className="dropdown">
        <div className="username-container" onClick={handleClick}>
          <p>{props.username}</p>
          <div>
            <i className="fa-solid fa-sort-down"></i>
          </div>
        </div>
        {displayMenu ? (
          <ul>
            <li onClick={handleShowPassword}>Change Password</li>
            <li onClick={handleShowLogout}>Logout</li>
          </ul>
        ) : (
          <div></div>
        )}
      </div>

      <Modal
        show={showLogout}
        onHide={handleCloseLogout}
        backdrop="static"
        keyboard={false}
        className="modal-log-out"
      >
        <Modal.Header>
          <p className="modal-title">Are you sure?</p>
        </Modal.Header>
        <Modal.Body>
          <p>Do you want to log out?</p>
          <div className="modal-button-group-container">
            <div className="modal-logout-button-container">
              <button className="btn" onClick={Logout}>
                Logout
              </button>
            </div>
            <div className="modal-cancel-button-container">
              <button className="btn" onClick={handleCloseLogout}>
                Cancel
              </button>
            </div>
          </div>
        </Modal.Body>
      </Modal>

      <Modal
        show={showPassword}
        onHide={handleClosePassword}
        backdrop="static"
        keyboard={false}
        className="modal-change-password"
      >
        <Modal.Header>
          <p className="modal-title">Change password</p>
        </Modal.Header>
        <Modal.Body>
          <Row>
            <Form.Label column lg={4}>
              Old Password
            </Form.Label>
            <Col>
              <Form.Control
                type="password"
                value={OldPassword}
                onChange={handleChangeOldPassword}
              />
            </Col>
          </Row>

          <Row>
            <Form.Label column lg={4}>
              New Password
            </Form.Label>
            <Col>
              <Form.Control
                type="password"
                value={NewPassword}
                onChange={handleChangeNewPassword}
              />
            </Col>
          </Row>
          <Row>
            <Form.Label column lg={4}>
              Confirm Password
            </Form.Label>
            <Col>
              <Form.Control
                type="password"
                value={ConfirmPassword}
                onChange={handleChangeConfirmPassword}
              />
            </Col>
          </Row>
          <div className="modal-change-password-error-list">
            <ul>
              {error.map((errors) => (
                <li>{errors}</li>
              ))}
            </ul>
          </div>
          <div className="modal-button-group-container">
            <div className="modal-logout-button-container">
              <button
                disabled={isDisabled()}
                className="btn"
                onClick={ChangePassword}
              >
                Save
              </button>
            </div>
            <div className="modal-cancel-button-container">
              <button className="btn" onClick={handleClosePassword}>
                Cancel
              </button>
            </div>
          </div>
        </Modal.Body>
      </Modal>

      <Modal
        show={showSuccess}
        onHide={handleCloseSuccess}
        className="modal-success"
      >
        <Modal.Header>
          <p className="modal-title">Change password</p>
        </Modal.Header>
        <Modal.Body>
          <p>Your password has been changed successfully!</p>
          <div className="modal-button-group-container">
            <div className="modal-cancel-button-container">
              <button className="btn" onClick={handleCloseSuccess}>
                Cancel
              </button>
            </div>
          </div>
        </Modal.Body>
      </Modal>
    </div>
  );
}

export default Navbar;
