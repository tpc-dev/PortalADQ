/** @type {import('tailwindcss').Config} */

const config = {
  content: [
    './src/pages/**/*.{js,jsx,ts,tsx}',
    './src/components/**/*.{js,jsx,ts,tsx}'
  ],
  theme: {
    container: {
      center: true
    },
    extend: {
      colors: {
        'app': '#1135A6'
      }
    }
  },
  plugins: []
}

module.exports = config
