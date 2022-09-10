import { Modal } from "antd";
import React, { useState, useEffect } from "react";
import axiosInstance from "../../axios";
import "./Modal.css";
const ReturnAssignmentModal = (props) => {
const [isModalVisibleReturn, setIsModalVisibleReturn] = useState(props.boolReturn);
const assignmentId = props.assignmentId;

useEffect(() => {
setIsModalVisibleReturn(props.boolReturn);
}, [props.boolReturn]);

const handleOkReturn = () => {
axiosInstance
    .put(`requests/create-request/${assignmentId}`)
    .then((response) => {
    setIsModalVisibleReturn(false);
    props.handleCancelReturn();
    props.handleReturn(response.id);
    });
};

return (
<>
    <Modal
    title="Are you sure?"
    visible={isModalVisibleReturn}
    closable={false}
    centered={true}
    okText="Yes"
    okType="danger"
    onOk={handleOkReturn}
    onCancel={props.handleCancelReturn}
    className="modalStyle"
    >
    <p>Do you want to create a returing request for this asset?</p>
    </Modal>
</>
);
};

export default ReturnAssignmentModal;
