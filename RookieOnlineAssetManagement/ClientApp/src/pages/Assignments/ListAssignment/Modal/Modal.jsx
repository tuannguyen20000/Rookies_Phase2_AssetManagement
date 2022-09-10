import { Modal } from "antd";
import React from "react";
import "./Modal.css";

const ModalAssignment = (props) => {
  const data = props.data;
  return (
    <>
      <Modal
        className="modalStyle"
        title="Detailed Assignment Information"
        visible={props.visible}
        onCancel={props.handleCancel}
        footer={null}
      >
        {data != null && (
          <div>
            <p>
              <span className="property">Asset Code </span>{" "}
              <span className="value">{data.assetCode}</span>
            </p>
            <p>
              <span className="property">Asset Name </span>{" "}
              <span className="value">{data.assetName}</span>
            </p>
            <p>
              <span className="property">Specification </span>{" "}
              <span className="value">{data.specification}</span>
            </p>
            <p>
              <span className="property">Assigned to </span>{" "}
              <span className="value">{data.assignedTo}</span>
            </p>
            <p>
              <span className="property">Assigned by </span>{" "}
              <span className="value">{data.assignedBy}</span>
            </p>
            <p>
              <span className="property">Assigned Date </span>{" "}
              <span className="value">{data.assignedDate}</span>
            </p>
            <p>
              <span className="property">State </span>{" "}
              <span className="value">{data.state}</span>
            </p>
            <p>
              <span className="property">Note </span>{" "}
              <span className="value">{data.note}</span>
            </p>
          </div>
        )}
      </Modal>
    </>
  );
};

export default ModalAssignment;
