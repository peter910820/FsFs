<template>
  <div class="not-found-page">
    <!-- 星空背景 -->
    <div class="background">
      <span
        v-for="(star, index) in stars"
        :key="index"
        class="star"
        :style="{
          top: star.top + '%',
          left: star.left + '%',
          width: star.size + 'px',
          height: star.size + 'px',
          animationDuration: star.duration + 's',
          opacity: star.opacity,
        }"
      ></span>
    </div>

    <!-- 中央文字 -->
    <div class="not-found-inner">
      <h1>404</h1>
      <p>抱歉，你訪問的頁面不存在</p>
      <button @click="goHome">回到首頁</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";

interface Star {
  top: number;
  left: number;
  size: number;
  duration: number;
  opacity: number;
}

const stars = ref<Star[]>([]);

const router = useRouter();

const generateStars = (count: number) => {
  const newStars: Star[] = [];
  for (let i = 0; i < count; i++) {
    newStars.push({
      top: Math.random() * 100, // 整個螢幕隨機
      left: Math.random() * 100,
      size: Math.random() * 3 + 1,
      duration: Math.random() * 3 + 2,
      opacity: Math.random() * 0.5 + 0.5,
    });
  }
  stars.value = newStars;
};

const goHome = () => {
  router.push("/");
};

onMounted(() => {
  generateStars(120); // 星星數量可調（加大一點才會滿版）
});
</script>

<style scoped>
.not-found-page {
  width: 100vw;
  height: 100vh;
  background-color: #1a1a2e;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  overflow: hidden;
}

/* 星空背景鋪滿 */
.background {
  position: absolute;
  width: 100%;
  height: 100%;
  top: 0;
  left: 0;
  pointer-events: none;
  z-index: 1;
}

.star {
  position: absolute;
  background: white;
  border-radius: 50%;
  animation: twinkle infinite ease-in-out;
}

/* 中央文字區塊 */
.not-found-inner {
  position: relative;
  z-index: 2;
  text-align: center;
  color: #fff;
  padding: 2rem;
}

.not-found-inner h1 {
  font-size: 6rem;
  color: #e94560;
  margin: 0;
  animation: float 2s ease-in-out infinite;
}

.not-found-inner p {
  font-size: 1.2rem;
  margin: 1rem 0;
}

.not-found-inner button {
  padding: 0.6rem 1.2rem;
  font-size: 1rem;
  border: none;
  background-color: #0f3460;
  color: #fff;
  border-radius: 5px;
  cursor: pointer;
  transition: all 0.3s ease;
}

.not-found-inner button:hover {
  background-color: #162447;
  transform: scale(1.05);
}

@keyframes float {
  0%,
  100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-15px);
  }
}

@keyframes twinkle {
  0%,
  100% {
    opacity: 0.2;
  }
  50% {
    opacity: 1;
  }
}
</style>
