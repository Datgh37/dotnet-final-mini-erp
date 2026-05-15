window.API_BASE = window.API_BASE || (window.location.hostname === 'localhost' ? 'https://localhost:44363/api' : '/api');

// Global Data Cache
window.AppData = {
    categories: [],
    brands: [],
    roles: [],
    customers: [],
    suppliers: []
};

// --- CORE FETCH WRAPPER ---
(function() {
    const originalFetch = window.fetch;
    window.fetch = async (...args) => {
        const url = args[0];
        const options = args[1] || {};
        
        // Auto-attach Bearer Token
        const token = localStorage.getItem('token');
        if (token) {
            options.headers = {
                ...options.headers,
                'Authorization': `Bearer ${token}`
            };
        }

        const method = options.method || 'GET';
        console.log(`%c[API Request] ${method} %c${url}`, 
            'color: #0d6efd; font-weight: bold; border-left: 3px solid #0d6efd; padding-left: 5px;', 
            'color: inherit;',
            options.body ? JSON.parse(options.body) : '');

        try {
            args[1] = options;
            const response = await originalFetch(...args);

            // Log response body without consuming original stream
            const responseClone = response.clone();
            let responseData = null;
            try {
                const contentType = response.headers.get("content-type");
                if (contentType && contentType.indexOf("application/json") !== -1) {
                    responseData = await responseClone.json();
                } else {
                    responseData = await responseClone.text();
                }
            } catch (e) { responseData = "[Unable to parse body]"; }

            const statusColor = response.ok ? '#198754' : '#dc3545';
            console.log(`%c[API Response] %c${url} %cStatus: ${response.status}`, 
                `color: ${statusColor}; font-weight: bold; border-left: 3px solid ${statusColor}; padding-left: 5px;`,
                'color: inherit;',
                `color: ${statusColor}; font-weight: bold;`,
                responseData);

            // Global Unauthorized Handling
            if (response.status === 401 && !url.includes('/Auth/login')) {
                console.warn('%c[API] Unauthorized access - redirecting to login', 'color: #fd7e14; font-weight: bold;');
                localStorage.clear();
                window.location.href = '/Auth/Login';
                return response;
            }

            return response;
        } catch (error) {
            console.error(`%c[API Error] %c${url}`, 'color: #dc3545; font-weight: bold;', 'color: inherit;', error);
            throw error;
        }
    };
})();

// --- UI UTILITIES ---

// Unified Table Renderer (Simple List, No Pagination as requested)
window.renderTable = function(containerId, data, renderRowCallback) {
    const container = document.getElementById(containerId);
    if (!container) return;

    if (!Array.isArray(data)) {
        console.warn(`renderTable: Data for ${containerId} is not an array:`, data);
        container.innerHTML = '<tr><td colspan="20" class="text-center text-muted">Không có dữ liệu hoặc lỗi API</td></tr>';
        return;
    }

    if (data.length === 0) {
        container.innerHTML = '<tr><td colspan="20" class="text-center text-muted">Danh sách trống</td></tr>';
        return;
    }

    container.innerHTML = data.map(renderRowCallback).join('');
};

// Populate Select Dropdowns
window.populateSelect = async function(selectId, endpoint, allText = '-- Chọn --') {
    const select = document.getElementById(selectId);
    if (!select) return;

    try {
        const response = await fetch(`${window.API_BASE}/${endpoint}`);
        if (!response.ok) return;
        const data = await response.json();
        
        let html = `<option value="">${allText}</option>`;
        data.forEach(item => {
            html += `<option value="${item.id}">${item.name || item.userName}</option>`;
        });
        select.innerHTML = html;
        return data;
    } catch (e) { console.error(`Populate ${selectId} failed:`, e); }
};

// Global API Error Handler
window.handleApiError = async function(response) {
    if (response.ok) return;
    let message = "Đã xảy ra lỗi!";
    try {
        const err = await response.json();
        console.error('[API Error Data]', err);
        
        // Handle ASP.NET Core Validation Errors (ModelState)
        if (err.errors) {
            message = Object.values(err.errors).flat().join('<br>');
        } else {
            message = err.message || err.title || JSON.stringify(err);
        }
    } catch {
        const text = await response.text();
        if (text) message = text;
    }
    window.showAlert(message, 'danger');
    throw new Error(message);
};

window.changePassword = async function(oldPassword, newPassword) {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    if (!user.id) return;

    try {
        const response = await fetch(`${window.API_BASE}/Users/${user.id}/password`, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ oldPassword, newPassword })
        });

        await handleApiError(response);
        showAlert("Đổi mật khẩu thành công!");
        return true;
    } catch (e) {
        console.error(e);
        return false;
    }
};

window.showAlert = function(message, type = 'success') {
    const existing = document.querySelector('.glass-toast');
    if (existing) existing.remove();

    const toast = document.createElement('div');
    toast.className = `glass-toast toast-${type} position-fixed top-0 start-50 translate-middle-x`;
    toast.style.zIndex = '9999';
    
    let icon = 'cil-check-circle';
    if (type === 'danger') icon = 'cil-x-circle';
    if (type === 'info') icon = 'cil-info';
    if (type === 'warning') icon = 'cil-warning';

    toast.innerHTML = `
        <div class="glass-container d-flex align-items-center px-3 py-2 shadow-lg rounded-pill border border-${type} border-opacity-25" style="min-width: 300px;">
            <i class="${icon} text-${type} fs-4 me-2"></i>
            <div class="flex-grow-1 fw-medium text-body">${message}</div>
            <button type="button" class="btn-close ms-2" onclick="this.parentElement.parentElement.remove()" style="font-size: 0.7rem;"></button>
        </div>
    `;
    
    document.body.appendChild(toast);
    setTimeout(() => toast.classList.add('show'), 10);

    setTimeout(() => {
        if(toast.parentElement) {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 400);
        }
    }, 5000);
};

window.showConfirm = function(title, message, onConfirm) {
    const modalId = 'globalConfirmModal';
    let modalEl = document.getElementById(modalId);
    if (!modalEl) {
        modalEl = document.createElement('div');
        modalEl.id = modalId;
        modalEl.className = 'modal fade';
        modalEl.setAttribute('tabindex', '-1');
        modalEl.innerHTML = `
            <div class="modal-dialog modal-dialog-centered shadow-lg">
                <div class="modal-content border-0 shadow">
                    <div class="modal-header border-0 pb-0">
                        <h5 class="modal-title fw-bold text-primary">${title}</h5>
                        <button type="button" class="btn-close" data-coreui-dismiss="modal"></button>
                    </div>
                    <div class="modal-body py-4">
                        <p class="mb-0 text-secondary">${message}</p>
                    </div>
                    <div class="modal-footer border-0 pt-0">
                        <button type="button" class="btn btn-light px-4" data-coreui-dismiss="modal">Hủy</button>
                        <button type="button" id="confirmBtn" class="btn btn-primary px-4 shadow-sm">Xác nhận</button>
                    </div>
                </div>
            </div>
        `;
        document.body.appendChild(modalEl);
    } else {
        modalEl.querySelector('.modal-title').innerText = title;
        modalEl.querySelector('.modal-body p').innerText = message;
    }

    const confirmBtn = modalEl.querySelector('#confirmBtn');
    // Remove old listeners
    const newConfirmBtn = confirmBtn.cloneNode(true);
    confirmBtn.parentNode.replaceChild(newConfirmBtn, confirmBtn);

    newConfirmBtn.addEventListener('click', () => {
        const modalInstance = coreui.Modal.getInstance(modalEl);
        if(modalInstance) modalInstance.hide();
        onConfirm();
    });

    new coreui.Modal(modalEl).show();
};

// --- AUTH UI ---
window.checkAuth = function() {
    const token = localStorage.getItem('token');
    const isLoginPage = window.location.pathname.toLowerCase().includes('/auth/login');
    
    // Toggle dropdown items
    const loginEl = document.getElementById('dropdown-login');
    const logoutEl = document.getElementById('dropdown-logout');
    
    if (token) {
        if (loginEl) loginEl.classList.add('d-none');
        if (logoutEl) logoutEl.classList.remove('d-none');
        
        const user = JSON.parse(localStorage.getItem('user') || '{}');
        const userNameEl = document.getElementById('header-user-name');
        if (userNameEl) userNameEl.innerText = user.fullName || user.userName;

        // Hide Users menu for non-Admin
        const usersMenu = document.getElementById('users-menu-item');
        if (usersMenu) {
            if (user.role === 'Admin') {
                usersMenu.classList.remove('d-none');
            } else {
                usersMenu.classList.add('d-none');
            }
        }

        // Redirect if on forbidden page (Staff trying to access /Users)
        if (user.role !== 'Admin' && window.location.pathname.toLowerCase().includes('/users')) {
            window.location.href = '/Dashboard';
        }
    } else {
        if (loginEl) loginEl.classList.remove('d-none');
        if (logoutEl) logoutEl.classList.add('d-none');
        
        if (!isLoginPage) {
            window.location.href = '/Auth/Login';
        }
    }
};

window.logout = function() {
    localStorage.clear();
    window.location.href = '/Auth/Login';
};

// Initialize
document.addEventListener('DOMContentLoaded', window.checkAuth);
