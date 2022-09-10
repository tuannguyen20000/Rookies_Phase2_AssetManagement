import { Modal } from "antd";
import React, { useState, useEffect } from "react";
import assignmentService from "../../../services/assignmentService";
import "./index.css";
const DeleteAssignmentModal = (props) => {
  const [isModalVisible, setIsModalVisible] = useState(props.bool);
  const assignmentId = props.assignmentId;

  useEffect(() => {
    setIsModalVisible(props.bool);
  }, [props.bool]);

  const handleOk = () => {
    assignmentService.deleteAssignment(assignmentId).then((response) => {
      setIsModalVisible(false);
      props.handleCancel();
      props.handleDelete(response.data.id);
    });
  };

  return (
    <>
      <Modal
        title="Are you sure?"
        visible={isModalVisible}
        closable={false}
        centered={true}
        okText="Delete"
        okType="danger"
        onOk={handleOk}
        onCancel={props.handleCancel}
        className="modalStyle"
      >
        <p>Do you want to delete this assignment!</p>
      </Modal>
    </>
  );
};

export default DeleteAssignmentModal;
