using UnityEngine;

//�������� ������ �ִ� Ÿ�Ե��� �������� �������ϴ� �������̽�
public interface IDamageAndHealable
{
    //�������� �����ų� ���������� �� �������̽��� ����ϰ� �Ʒ� �޼������ �� �����ؾ���
    //TakeDamage �޼���� �޴� ������ ��, ���� ����, ���� ǥ���� ���� ���� �Ķ���ͷ� �޴´�.
    void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);//Vector3�� ��ƼŬ ����� ���� ���̴�.
    //
    void TakeHeal(float healAmount);
}
