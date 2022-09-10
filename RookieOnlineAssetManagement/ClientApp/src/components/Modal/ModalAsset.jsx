import { Modal } from "antd";
import React from "react";
import "./Modal.css";
import moment from "moment";

const ModalAsset = (props) => {
  const date = new Date();
  const data = props.data;
  const history = props.history.assignments;
  return (
    <>
      <Modal
        className="modalStyle"
        title="Detailed Asset Information"
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
              <span className="property">Category </span>{" "}
              <span className="value">{data.category}</span>
            </p>
            <p>
              <span className="property">Installed Date </span>{" "}
              <span className="value">{data.installedDate}</span>
            </p>
            <p>
              <span className="property">State </span>{" "}
              <span className="value">{data.state}</span>
            </p>
            <p>
              <span className="property">Location </span>{" "}
              <span className="value">{data.location}</span>
            </p>
            <p>
              <span className="property">Specification </span>{" "}
              <span className="value">{data.specification}</span>
            </p>
            <div className="row">
              <span className="property col-2 p-0">History </span>
              <div className="col-9">
                <table className="table">
                  <thead>
                    <tr>
                      <th>Assigned Date</th>
                      <th>Assigned To</th>
                      <th>Assigned By</th>
                      <th>Returned Date</th>
                    </tr>
                  </thead>
                  <tbody>
                    {history ? (
                      history.map((item) => (
                        <tr>
                          <td>
                            {moment(item.assignedDate).format("DD/MM/YYYY")}
                          </td>
                          <td>{item.assignedTo}</td>
                          <td>{item.assignedBy}</td>
                          {item.requestState == "Completed" ? (
                            <td>
                              {moment(item.returnedDate).format("DD/MM/YYYY")}
                            </td>
                          ) : (
                            <td>Updating...</td>
                          )}
                        </tr>
                      ))
                    ) : (
                      <tr>
                        <td>No history</td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        )}
      </Modal>
    </>
  );
};

export default ModalAsset;
