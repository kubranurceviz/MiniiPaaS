import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";

const Dashboard = () => {
  const { logout, token } = useContext(AuthContext);

  return (
    <div style={{ padding: "2rem" }}>
      <h1>Dashboard</h1>
      <p>JWT Token: {token}</p>
      <button onClick={logout}>Çıkış Yap</button>
    </div>
  );
};

export default Dashboard;