<script setup lang="ts">
import { onMounted, ref } from "vue";
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
  const next: Star[] = [];
  for (let i = 0; i < count; i++) {
    next.push({
      top: Math.random() * 100,
      left: Math.random() * 100,
      size: Math.random() * 2.5 + 1,
      duration: Math.random() * 3 + 2,
      opacity: Math.random() * 0.45 + 0.35,
    });
  }
  stars.value = next;
};

const goHome = () => {
  router.push("/");
};

onMounted(() => {
  generateStars(120);
});
</script>

<template>
  <div class="not-found-page">
    <div class="background" aria-hidden="true">
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
      />
    </div>

    <div class="not-found-inner">
      <v-avatar class="mb-6" color="primary" size="72" variant="tonal">
        <v-icon icon="mdi-map-search-outline" size="36" />
      </v-avatar>

      <div class="code-label text-h2 font-weight-bold mb-3">404</div>
      <div class="text-h6 font-weight-medium mb-2">找不到頁面</div>
      <p class="text-body-1 text-medium-emphasis mb-8">抱歉，你訪問的頁面不存在或已被移除。</p>

      <v-btn rounded="xl" size="large" color="primary" variant="flat" prepend-icon="mdi-home" @click="goHome">
        回到首頁
      </v-btn>
    </div>
  </div>
</template>

<style scoped>
.not-found-page {
  width: 100vw;
  height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  overflow: hidden;
  background:
    radial-gradient(ellipse at 30% 20%, rgba(var(--v-theme-primary), 0.18), transparent 55%),
    radial-gradient(ellipse at 70% 80%, rgba(var(--v-theme-accent), 0.12), transparent 50%),
    rgb(var(--v-theme-background));
  color: rgb(var(--v-theme-on-background));
}

.background {
  position: absolute;
  inset: 0;
  pointer-events: none;
  z-index: 1;
}

.star {
  position: absolute;
  border-radius: 50%;
  background: rgb(var(--v-theme-on-background));
  animation: twinkle infinite ease-in-out;
}

.not-found-inner {
  position: relative;
  z-index: 2;
  text-align: center;
  padding: 2rem;
  max-width: 28rem;
}

.code-label {
  letter-spacing: 0.08em;
  color: rgb(var(--v-theme-primary));
  animation: float 2.4s ease-in-out infinite;
}

@keyframes float {
  0%,
  100% {
    transform: translateY(0);
  }
  50% {
    transform: translateY(-12px);
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
