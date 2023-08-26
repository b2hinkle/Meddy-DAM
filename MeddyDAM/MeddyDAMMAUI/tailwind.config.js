/** @type {import('tailwindcss').Config} */
module.exports = {
    mode: 'jit',
    content: [
        './wwwroot/index.html',
        './**/*.razor'
    ],
    darkMode: true,
    variants: {
        extend: {},
    },
    theme: {
        extend: {},
    },
    plugins: [],
}

