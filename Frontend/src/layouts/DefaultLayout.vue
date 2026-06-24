<template>
  <div class="main-style">
    <div class="sticky-navbar">
      <NavBar />
    </div>
    <div class="row">
      <div class="col m12 m12 s12">
        <router-view />
        <div id="modal1" class="modal">
          <div class="modal-content">
            <h1>登入</h1>
            <form>
              <div class="input-field">
                <i class="material-icons prefix">account_circle</i>
                <input id="icon_prefix" v-model="form.username" type="text" class="validate" required />
                <span class="helper-text" data-error="此欄位不能為空" data-success=""></span>
                <label for="icon_prefix">username</label>
              </div>
              <div class="input-field">
                <i class="material-icons prefix">lock</i>
                <input id="icon_lock" v-model="form.password" type="password" class="validate" required />
                <span class="helper-text" data-error="此欄位不能為空" data-success=""></span>
                <label for="icon_lock">password</label>
              </div>
              <div>
                <button @click="handleSubmit" class="modal-close button-common flex-btn" type="button">
                  點我登入
                  <i class="material-icons right">send</i>
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import NavBar from "@/components/NavBar.vue";

import axios from "axios";
import { ref } from "vue";
import { useRouter } from "vue-router";

import type { ResponseType } from "@/types/response";

const router = useRouter();
const form = ref({
  username: "",
  password: "",
});

const handleSubmit = async () => {
  try {
    const apiUrl = import.meta.env.VITE_API_DOMAIN ? `${import.meta.env.VITE_API_DOMAIN}/api/login` : "/api/login";
    const response = await axios.post<ResponseType<string>>(apiUrl, form.value, {
      withCredentials: true,
    });
    sessionStorage.setItem("errorMsg", response.data.msg);
    // userStore.set(response.data.data);
    router.push("/");
  } catch (error) {
    if (axios.isAxiosError(error)) {
      sessionStorage.setItem("errorMsg", `${error.response?.status}: ${error.response?.data.msg}`);
    } else {
      sessionStorage.setItem("errorMsg", String(error));
    }
    router.push("/error");
  }
};
</script>

<style>
.main-style {
  min-height: 100vh;
  min-width: 600px;
  font-family: "madoufmg";

  background-color: #f2ebea;
  background-size: 400% 400%;
}

.modal {
  border: 2px transparent f2ebea !important;
  border-radius: 20px !important;
  max-width: 800px;
}

.modal-content {
  background-color: #f2ebea;
}

.row {
  height: 100%;
  margin-bottom: 0px !important;
  padding: 30px;
}
.main-block {
  padding: 40px;
}
.sub-block {
  padding-top: 20px;
  margin-top: 10px;
  cursor: default !important;
  border: 2px solid f2ebea;
  border-radius: 20px;
}
.sticky-navbar {
  position: sticky;
  top: 0px;
  z-index: 10;
}
.browser-default {
  background-color: #f2ebea;
  border: 0px;
  border-bottom: 1px solid rgb(118, 118, 118);
}
/* hide materializecss select in mobile */
.mobile-hidden {
  display: block !important;
}
.mobile-display {
  display: none !important;
}
@media (max-width: 768px) {
  .mobile-display {
    display: block !important;
  }
  .mobile-hidden {
    display: none !important;
  }
}

/* Button Style */
.button-common {
  margin: 10px;
  padding: 15px 30px;
  text-align: center;
  text-transform: uppercase;
  transition: 0.5s;
  background-size: 200% auto;
  color: white;
  border-radius: 10px;
  display: block;
  border: 0px;
  font-weight: 700;
  box-shadow: 0px 0px 14px -7px #f09819;
  background-image: linear-gradient(45deg, #ff512f 0%, #f09819 51%, #ff512f 100%);
  cursor: pointer;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
}

.button-common:hover {
  background-position: right center;
  /* change the direction of the change here */
  color: #fff;
  text-decoration: none;
}

.button-common:active {
  transform: scale(0.95);
}

.flex-btn {
  display: flex;
  align-items: center;
  justify-content: center;
}

/* font-settings */
@font-face {
  font-family: "madoufmg";
  src: url("@/assets/fonts/madoufmg.ttf") format("truetype");
}
</style>
