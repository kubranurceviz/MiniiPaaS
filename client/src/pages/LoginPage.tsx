import {
  MDBBtn,
} from "mdb-react-ui-kit";
import { useContext, useState } from "react";
import { AuthContext } from "../context/AuthContext";
import "../App.css";

// Font Awesome CDN'i public/index.html'e eklemelisiniz:
// <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

const LoginPage = () => {
  const { login } = useContext(AuthContext);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const handleLogin = async () => {
    try {
      await login(email, password);
    } catch {
      alert("Login failed. Check your credentials.");
    }
  };

  return (
    <div className="login-page">
      <img
        src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw2.webp"
        alt="Login visual"
        className="login-image"
      />
      <div className="login-box">
        <h3 className="login-title">Login</h3>
        <div className="input-row">
          <label htmlFor="formEmail" className="input-label">
            Email address
          </label>
          <input
            className="input-field form-control"
            id="formEmail"
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
        </div>
        <div className="input-row">
          <label htmlFor="formPassword" className="input-label">
            Password
          </label>
          <input
            className="input-field form-control"
            id="formPassword"
            type={showPassword ? "text" : "password"}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            autoComplete="current-password"
          />
          <button
            type="button"
            className="password-eye"
            tabIndex={-1}
            onClick={() => setShowPassword((v) => !v)}
            aria-label={showPassword ? "Hide password" : "Show password"}
          >
            {showPassword ? (
              <i className="fa-regular fa-eye" />
            ) : (
              <i className="fa-regular fa-eye-slash" />
            )}
          </button>
        </div>
        <div className="login-btn-wrapper">
          <MDBBtn className="w-100" size="lg" onClick={handleLogin}>
            Login
          </MDBBtn>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />