import { Modal } from "antd";
import React from "react";
import ListAsset from "./ListAsset";
import "./ModalAsset.css";
const ModalAsset = (props) => {
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
        <ListAsset
          onSelectedAsset={props.selectedAsset}
          DefaultAsset={props.defaultAsset}
        />
      </Modal>
    </>
  );
};

export default ModalAsset;
