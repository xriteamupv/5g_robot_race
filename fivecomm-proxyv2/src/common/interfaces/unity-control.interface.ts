export interface UnityControl {
  header: 'control',
  data: {
    linear: {
      x: number,
      y: number,
      z: number
    },
    angular: {
      x: number,
      y: number,
      z: number
    }
  }
}