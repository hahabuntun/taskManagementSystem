import { theme } from 'antd';

const { defaultAlgorithm } = theme;

export const getThemeConfig = (isDarkMode: boolean) => ({
    algorithm: isDarkMode ? theme.darkAlgorithm : defaultAlgorithm,
    token: {
        colorPrimary: '#1677ff',
        colorText: isDarkMode ? '#ffffff' : '#1f2a44',
        colorTextSecondary: isDarkMode ? '#d1d5db' : '#6b7280',
        colorTextTertiary: isDarkMode ? '#9ca3af' : '#9ca3af',
        colorTextQuaternary: isDarkMode ? '#6b7280' : '#d1d5db',
        colorBorder: isDarkMode ? '#374151' : '#d1d5db',
        colorBorderSecondary: isDarkMode ? '#4b5563' : '#e5e7eb',
        borderThin: isDarkMode ? '1px solid #374151' : '1px solid #d1d5db',
        borderRadius: 6,
        fontSize: 16,
        colorTextPlaceholder: '#9ca3af',
        colorBgContainer: isDarkMode ? '#1f2937' : '#ffffff',
        colorBgElevated: isDarkMode ? '#374151' : '#f9fafb',
        colorBgHover: isDarkMode ? '#4b5563' : '#eff6ff',
        colorSuccess: '#10b981',
        colorError: '#ef4444',
        colorInfo: '#3b82f6',
        colorTextTooltip: isDarkMode ? '#ffffff' : '#1f2a44', // Explicit tooltip text color
    },
    components: {
        Input: {
            colorTextPlaceholder: '#9ca3af',
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
        },
        Table: {
            colorBorder: undefined,
            colorBorderSecondary: undefined,
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
        },
        List: {
            colorBorder: undefined,
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
        },
        Collapse: {
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
        },
        Typography: {
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            colorTextDescription: isDarkMode ? '#d1d5db' : '#6b7280',
            fontSize: 16,
        },
        Button: {
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
            colorPrimaryHover: '#3b82f6',
        },
        Select: {
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            colorTextPlaceholder: '#9ca3af',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
        },
        Menu: {
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            colorBg: isDarkMode ? '#1f2937' : '#ffffff',
            colorItemBgHover: isDarkMode ? '#4b5563' : '#eff6ff',
            colorPopupText: isDarkMode ? '#ffffff' : '#1f2a44',
            colorPopupBg: isDarkMode ? '#4b5563' : '#f9fafb',
            colorItemText: isDarkMode ? '#ffffff' : '#1f2a44', // Explicit item text color
            fontSize: 16,
        },
        Card: {
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
            colorBgContainer: isDarkMode ? '#374151' : '#f9fafb',
        },
        Notification: {
            colorBgElevated: isDarkMode ? '#374151' : '#ffffff',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            colorTextHeading: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
        },
        Tooltip: {
            colorBgSpotlight: isDarkMode ? '#4b5563' : '#f9fafb',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            colorTextLight: isDarkMode ? '#ffffff' : '#1f2a44',
            colorBgDefault: isDarkMode ? '#4b5563' : '#f9fafb', // Reinforce tooltip background
            fontSize: 16,
        },
        Tag: {
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
        },
        Popconfirm: {
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
        },
        Badge: {
            colorText: isDarkMode ? '#ffffff' : '#1f2a44',
            fontSize: 16,
            colorBgBase: '#ef4444',
        },
        Divider: {
            colorBorder: isDarkMode ? '#374151' : '#d1d5db',
            fontSize: 16,
        },
    },
});