import { Modal } from "antd";
import React from "react";
import moment from "moment";
import "./Modal.css";

const ModalExample = (props) => {
  const data = props.data;
  return (
    <>
      <Modal
        className="modalStyle"
        title="Detailed User Information"
        visible={props.visible}
        onCancel={props.handleCancel}
        footer={null}
      >
        {data != null && (
          <div>
            <p>
              <span className="property">Staff Code </span>{" "}
              <span className="value">{data.staffCode}</span>
            </p>
            <p>
              <span className="property">Full Name </span>{" "}
              <span className="value">{data.fullName}</span>
            </p>
            <p>
              <span className="property">Username </span>{" "}
              <span className="value">{data.userName}</span>
            </p>
            <p>
              <span className="property">Date Of Birth </span>{" "}
              <span className="value">
                {moment(data.dateOfBirth).format("DD/MM/YYYY")}
              </span>
            </p>
            <p>
              <span className="property">Gender </span>{" "}
              <span className="value">{data.gender}</span>
            </p>
            <p>
              <span className="property">Joined Date </span>{" "}
              <span className="value">{data.joinedDate}</span>
            </p>
            <p>
              <span className="property">Type </span>{" "}
              <span className="value">{data.type}</span>
            </p>
            <p>
              <span className="property">Location </span>{" "}
              <span className="value">{data.location}</span>
            </p>
          </div>
        )}
      </Modal>
    </>
  );
};

export default ModalExample;
