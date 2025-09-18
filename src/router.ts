import { createRouter, createWebHistory } from "vue-router"

const HomePage = () => import("./pages/HomePage.vue")
const LoginPage = () => import("./pages/LoginPage.vue")
const SignupPage = () => import("./pages/SignupPage.vue")
const NotesPage = () => import("./pages/NoteManagementPage.vue")

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: "/", name: "home", component: HomePage },
    { path: "/login", name: "login", component: LoginPage },
    { path: "/signup", name: "signup", component: SignupPage },
    {
      path: "/notes",
      name: "notes",
      component: NotesPage,
      meta: { auth: true }
    },
    { path: "/:pathMatch(.*)*", redirect: "/" } // catch-all
  ]
})

// Optional guard example (only runs on routes with meta.auth)
// router.beforeEach(to => {
//   if (to.meta.auth && !localStorage.getItem('accessToken')) {
//     return { name: 'login', query: { redirect: to.fullPath } }
//   }
// })

export default router
