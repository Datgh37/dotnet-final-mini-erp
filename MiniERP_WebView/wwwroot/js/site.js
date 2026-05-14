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
        message = err.message || err.title || JSON.stringify(err);
    } catch {
        const text = await response.text();
        if (text) message = text;
    }
    window.showAlert(message, 'danger');
    throw new Error(message);
};

window.showAlert = function(message, type = 'success') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show position-fixed top-0 start-50 translate-middle-x mt-3`;
    alertDiv.style.zIndex = '9999';
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-coreui-dismiss="alert" aria-label="Close"></button>
    `;
    document.body.appendChild(alertDiv);
    setTimeout(() => {
        const bsAlert = new coreui.Alert(alertDiv);
        bsAlert.close();
    }, 3000);
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
