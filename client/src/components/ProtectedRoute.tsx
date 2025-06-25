import { useContext } from 'react';
import { Navigate, useParams } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';
import type { SystemRole } from '../services/roleService';

interface ProtectedRouteProps {
  children: React.ReactNode;
  roles?: SystemRole[];
  sameCompany?: boolean;
}

const ProtectedRoute = ({ children, roles, sameCompany }: ProtectedRouteProps) => {
  const { user, isRoleLoaded } = useContext(AuthContext);
  const { companyId: routeCompanyId } = useParams();

  if (!isRoleLoaded) {
    return <div>Loading authorization...</div>;
  }

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  if (roles && !roles.includes(user.role)) {
    return <Navigate to="/unauthorized" replace />;
  }

  if (sameCompany && routeCompanyId && parseInt(routeCompanyId) !== user.companyId) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;