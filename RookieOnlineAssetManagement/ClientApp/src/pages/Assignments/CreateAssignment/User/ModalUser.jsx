import { Modal } from "antd";
import React from "react";
import ListUser from "./ListUser";
import "./ModalUser.css";
const ModalUser = (props) => {
  return (
    <>
      <Modal
        visible={props.visible}
        onCancel={props.handleCancel}
        onOk={props.handleCancel}
        closable={false}
        width={800}
        className="modalStyle"
      >
        <ListUser
          onSelectedUser={props.selectedUser}
          DefaultUser={props.defaultUser}
        />
      </Modal>
    </>
  );
};

export default ModalUser;
