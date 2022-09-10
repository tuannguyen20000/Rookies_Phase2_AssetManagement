import React, { useEffect, useState } from "react";
import axiosInstance from "../../../axios";

import Modal from "antd/lib/modal/Modal";

import "./index.css";

const DisableUser = (props) => {
  const [modal, setModal] = useState(props.bool);
  const [checkAssgignValid, setCheckAssignValid] = useState();

  useEffect(() => {
    setModal(props.bool);
    if (props.id != null) {
      axiosInstance
        .get(`users/checkValidAssignments/${props.id}`)
        .then((response) => {
          if (response === false) {
            setCheckAssignValid(false);
          } else {
            setCheckAssignValid(true);
          }
        });
    }
  }, [props.bool]);

  const handleDisableUser = () => {
    axiosInstance.put(`users/disable-user/${props.id}`).then((response) => {
      if (response === true) {
        props.handleCancel();
        window.location.reload();
      }
    });
  };

  return (
    <div>
      {!checkAssgignValid ? (
        <Modal
          className="modalStyle"
          title="Are you sure"
          visible={modal}
          onCancel={props.handleCancel}
          footer={null}
        >
          <div>
            <p className="modal-body">Do you want to disable User?</p>
            <div className="modal-button-group-container">
              <div className="modal-disable-button-container">
                <button className="btn" onClick={handleDisableUser}>
                  Disable
                </button>
              </div>
              <div className="modal-cancel-button-container">
                <button className="btn" onClick={props.handleCancel}>
                  Cancel
                </button>
              </div>
            </div>
          </div>
        </Modal>
      ) : (
        <Modal
          className="modalStyle"
          title="Can not disable user"
          visible={modal}
          onCancel={props.handleCancel}
          footer={null}
        >
          <div>
            <p>
              There are valid assignments belonging to this user. Please close
              all assignments before disabling user.
            </p>
          </div>
        </Modal>
      )}
    </div>
  );
};

export default DisableUser;
