export const horizontalBarChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    indexAxis: 'y' as const,
    plugins: {
        legend: {
            position: 'top' as const,
            labels: {
                color: '#000000', // Black legend text
                font: {
                    size: 18, // Larger font size
                },
            },
        },
        tooltip: {
            enabled: true,
            titleFont: {
                size: 18, // Larger tooltip title
            },
            bodyFont: {
                size: 16, // Larger tooltip body
            },
            titleColor: '#000000', // Black tooltip title
            bodyColor: '#000000', // Black tooltip body
        },
    },
    scales: {
        x: {
            title: {
                display: false,
            },
            stacked: true,
            grid: {
                color: '#333333', // Darker grid lines
            },
            ticks: {
                color: '#000000', // Black tick labels
                font: {
                    size: 16, // Larger font size
                },
            },
        },
        y: {
            title: {
                display: false,
            },
            stacked: true,
            grid: {
                color: '#333333', // Darker grid lines
            },
            ticks: {
                color: '#000000', // Black tick labels
                font: {
                    size: 16, // Larger font size
                },
            },
        },
    },
};

export const horizontalBarChartWithoutHeadingOptions = {
    responsive: true,
    maintainAspectRatio: false,
    indexAxis: 'y' as const,
    plugins: {
        legend: {
            display: false,
        },
        tooltip: {
            enabled: true,
            titleFont: {
                size: 18,
            },
            bodyFont: {
                size: 16,
            },
            titleColor: '#000000',
            bodyColor: '#000000',
        },
        title: {
            display: false,
        },
    },
    scales: {
        x: {
            title: {
                display: false,
            },
            stacked: true,
            grid: {
                color: '#333333',
            },
            ticks: {
                color: '#000000',
                font: {
                    size: 16,
                },
            },
        },
        y: {
            title: {
                display: false,
            },
            stacked: true,
            grid: {
                color: '#333333',
            },
            ticks: {
                color: '#000000',
                font: {
                    size: 16,
                },
            },
        },
    },
};

export const pieChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
        legend: {
            position: 'top' as const,
            labels: {
                color: '#000000', // Black legend text
                font: {
                    size: 18, // Larger font size
                },
            },
        },
        tooltip: {
            enabled: true,
            titleFont: {
                size: 18,
            },
            bodyFont: {
                size: 16,
            },
            titleColor: '#000000',
            bodyColor: '#000000',
        },
    },
};