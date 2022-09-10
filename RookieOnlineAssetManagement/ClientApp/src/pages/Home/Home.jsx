import React, { useEffect, useState } from "react";
import moment from "moment";

import assignmentService from "../../services/assignmentService";
import Assignment from "./Assignment";

import { Table, Typography } from "antd";
import Modal from "react-bootstrap/Modal";
import "./Home.css";

function Home(props) {
  const [data, setData] = useState();
  const [loading, setLoading] = useState(false);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [info, setInfo] = useState();
  const [showReturn, setShowReturn] = useState(false);
  const [showAccept, setShowAccept] = useState(false);
  const [showDecline, setShowDecline] = useState(false);
  const [id, setId] = useState();

  const fetchData = (params = {}) => {
    setLoading(true);
    assignmentService
      .getDetail(props.userId, params.sortOrder, params.sortField)
      .then((result) => {
        result.data.map((x) => {
          x.assignedDate = moment(x.assignedDate).format("DD-MM-YYYY");
          if (x.state === "WaitingForAcceptance")
            x.state = "Waiting For Acceptance";
          else x.state = "Accepted";
          if (x.requestState === "WaitingForReturning")
            x.requestState = "Waiting For Returning";
        });
        setData(result.data);
        setLoading(false);
      })
      .catch((e) => console.log(e));
  };

  const onChange = (pagination, filters, sorter, extra) => {
    fetchData({
      sortField: sorter.field,
      sortOrder: sorter.order,
      ...filters,
    });
  };

  const columnsDetail = [
    {
      title: "Asset Code",
      dataIndex: "assetCode",
      width: "10%",
      sorter: true,
    },
    {
      title: "Asset Name",
      dataIndex: "assetName",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Assigned to",
      dataIndex: "assignedTo",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Assigned by",
      dataIndex: "assignedBy",
      ellipsis: true,
      sorter: true,
    },
    {
      title: "Assigned Date",
      dataIndex: "assignedDate",
      width: "15%",
      sorter: true,
    },
    {
      title: "State",
      dataIndex: "state",
      width: "20%",
      sorter: true,
    },
    {
      title: "Action",
      dataIndex: "id",
      key: "id",
      width: "12%",
      render: (id, record) =>
        record.state === "Waiting For Acceptance" ? (
          <div className="home-button-group">
            <i
              className="fa-solid fa-check fa-xl home-accept-button"
              onClick={(e) => {
                e.stopPropagation();
                setShowAccept(true);
                setId(id);
              }}
            ></i>
            <i
              className="fa-solid fa-xmark fa-xl home-cancel-button"
              onClick={(e) => {
                e.stopPropagation();
                setId(id);
                setShowDecline(true);
              }}
            ></i>
            <i className="fa-solid fa-rotate-left fa-lg home-return-button-disabled"></i>
          </div>
        ) : record.requestState === "Waiting For Returning" ? (
          <div className="home-button-group">
            <i className="fa-solid fa-check fa-xl home-accept-button-disabled"></i>
            <i className="fa-solid fa-xmark fa-xl home-cancel-button-disabled"></i>
            <i className="fa-solid fa-rotate-left fa-lg home-return-button-disabled"></i>
          </div>
        ) : (
          <div className="home-button-group">
            <i className="fa-solid fa-check fa-xl home-accept-button-disabled"></i>
            <i className="fa-solid fa-xmark fa-xl home-cancel-button-disabled"></i>
            <i
              className="fa-solid fa-rotate-left fa-lg home-return-button"
              onClick={(event) => {
                event.stopPropagation();
                setId(id);
                setShowReturn(true);
              }}
            ></i>
          </div>
        ),
    },
  ];

  useEffect(() => {
    fetchData();
  }, []);

  const handleReturn = () => {
    assignmentService.requestCreate(id).then(() => {
      fetchData();
      setShowReturn(false);
    });
  };

  const handleAccept = () => {
    assignmentService.acceptAssignment(id).then(() => {
      fetchData();
      setShowAccept(false);
    });
  };

  const handleDecline = () => {
    assignmentService.declineAssignment(id).then(() => {
      fetchData();
      setShowDecline(false);
    });
  };

  const handleCancel = () => {
    setIsModalVisible(false);
  };

  const showModal = (e) => {
    setIsModalVisible(true);
    setInfo(e);
  };

  return (
    <>
      <div className="home-table">
        <Typography className="header-home-list">My Assignment</Typography>
        <Table
          dataSource={data}
          columns={columnsDetail}
          loading={loading}
          onChange={onChange}
          onRow={(record, rowIndex) => {
            return {
              onClick: () => {
                if (record.state != "Waiting For Acceptance") {
                  showModal(record);
                }
              },
            };
          }}
        />
        <Assignment
          visible={isModalVisible}
          handleCancel={handleCancel}
          data={info}
        />
      </div>
      <div className="home-modal">
        <Modal
          show={showReturn}
          onHide={handleReturn}
          backdrop="static"
          onChange={onChange}
          keyboard={false}
        >
          <Modal.Header>
            <Modal.Title>
              <p>Are you sure?</p>
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <p> Do you want to create a returning request for this asset?</p>
            <div className="modal-return-button-container">
              <button
                className="btn assignment-accept-button"
                style={{ width: "13%" }}
                onClick={handleReturn}
              >
                Yes
              </button>
              <button
                className="btn assignment-cancel-button"
                style={{ width: "13%" }}
                onClick={() => setShowReturn(false)}
              >
                No
              </button>
            </div>
          </Modal.Body>
        </Modal>
      </div>
      <div className="home-modal">
        <Modal
          show={showDecline}
          onHide={handleReturn}
          backdrop="static"
          onChange={onChange}
          keyboard={false}
        >
          <Modal.Header>
            <Modal.Title>
              <p>Are you sure?</p>
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <p> Do you want to decline this assignment?</p>
            <div className="modal-return-button-container">
              <button
                className="btn assignment-accept-button"
                onClick={handleDecline}
              >
                Decline
              </button>
              <button
                className="btn assignment-cancel-button"
                onClick={() => setShowDecline(false)}
              >
                Cancel
              </button>
            </div>
          </Modal.Body>
        </Modal>
      </div>
      <div className="home-modal">
        <Modal
          show={showAccept}
          onHide={handleReturn}
          backdrop="static"
          onChange={onChange}
          keyboard={false}
        >
          <Modal.Header>
            <Modal.Title>
              <p>Are you sure?</p>
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <p> Do you want to accept this assignment?</p>
            <div className="modal-return-button-container">
              <button
                className="btn assignment-accept-button"
                onClick={handleAccept}
              >
                Accept
              </button>
              <button
                className="btn assignment-cancel-button"
                onClick={() => setShowAccept(false)}
              >
                Cancel
              </button>
            </div>
          </Modal.Body>
        </Modal>
      </div>
    </>
  );
}

export default Home;
