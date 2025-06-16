import React, { useState, useContext } from "react";
import {
  MDBContainer,
  MDBCol,
  MDBRow,
  MDBBtn,
  MDBInput,
} from "mdb-react-ui-kit";
import { AuthContext } from "../context/AuthContext";

export default function LoginPage() {
  const { login } = useContext(AuthContext);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await login(email, password);
      // yönlendirme yapılabilir
    } catch (err) {
      alert("Giriş başarısız.");
    }
  };

  return (
    <MDBContainer fluid className="p-3 my-5 h-custom">
      <MDBRow className="d-flex justify-content-center align-items-center">
        <MDBCol md="5">
          <img
            src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/draw2.webp"
            className="img-fluid"
            alt="Login illustration"
          />
        </MDBCol>

        <MDBCol md="4">
          <form onSubmit={handleSubmit}>
            <h3 className="mb-4 text-center">Giriş Yap</h3>
            <MDBInput
              wrapperClass="mb-4"
              label="Email"
              type="email"
              size="lg"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <MDBInput
              wrapperClass="mb-4"
              label="Şifre"
              type="password"
              size="lg"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />

            <div className="text-center mt-4">
              <MDBBtn type="submit" className="px-5" size="lg">
                Giriş Yap
              </MDBBtn>
            </div>
          </form>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  );
}