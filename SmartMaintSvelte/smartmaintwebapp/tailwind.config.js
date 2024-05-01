/** @type {import('tailwindcss').Config} */
export default {
  content: ['./src/**/*.{html,js,svelte,ts}'],
  theme: {
    extend: {
      textColor: {
        default: 'cyan.50', 
      },
      // {
      //   colors.: {
      //    theme(colors.gray.500);
      //   }
      // },
    },
  },
};
