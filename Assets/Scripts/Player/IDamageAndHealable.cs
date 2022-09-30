using UnityEngine;

//데미지를 입을수 있는 타입들이 공통으로 가져야하는 인터페이스
public interface IDamageAndHealable
{
    //데미지를 입히거나 힐받으려면 이 인터페이스를 상속하고 아래 메서드들을 꼭 구현해야함
    //TakeDamage 메서드는 받는 데미지 량, 적중 지점, 맞은 표면의 방향 값을 파라미터로 받는다.
    void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);//Vector3는 파티클 재생을 위한 값이다.
    //
    void TakeHeal(float healAmount);
}
