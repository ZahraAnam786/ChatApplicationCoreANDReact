
import { toast } from 'react-toastify';

const notificationService = {
	Success: (msg: string) => toast.success(msg),
	Error: (msg: string) => toast.error(msg),
	Info: (msg: string) => toast.info(msg),
	Warn: (msg: string) => toast.warn(msg)
};

export default notificationService;
