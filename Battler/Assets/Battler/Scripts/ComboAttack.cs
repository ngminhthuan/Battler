using UnityEngine;

public class ComboAttack
{
    Animator animator;
    int comboStep = 0;
    float lastAttackTime;
    public float comboDelay = 1.2f;
    bool inputBuffered = false;

    public ComboAttack(Animator animator)
    {
        this.animator = animator;
    }

    public void ComboUpdate()
    {
        if (Time.time - lastAttackTime > comboDelay)
        {
            ResetCombo();
        }

        HandleBufferedCombo();
        CheckAndStopCurrentAttack();
    }

    public void OnAttackInput()
    {
        if (comboStep == 0)
        {
            comboStep = 1;
            PlayComboAnimation(comboStep);
            lastAttackTime = Time.time;
        }
        else
        {
            inputBuffered = true;
        }
    }

    void HandleBufferedCombo()
    {
        if (!inputBuffered) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        string currentState = $"Anim_Attack{comboStep}";
        if (state.IsName(currentState) && state.normalizedTime > 0.6f)
        {
            comboStep++;
            if (comboStep > 3)
            {
                ResetCombo();
                return;
            }

            PlayComboAnimation(comboStep);
            lastAttackTime = Time.time;
            inputBuffered = false;
        }
    }

    void PlayComboAnimation(int step)
    {
        animator.SetBool("Attack" + step, true);
        if (step > 1)
            animator.SetBool("Attack" + (step - 1), false);
    }

    void CheckAndStopCurrentAttack()
    {
        for (int i = 1; i <= 3; i++)
        {
            string stateName = $"Anim_Attack{i}";
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                StopComboAnimation(i);
            }
        }
    }

    void StopComboAnimation(int step)
    {
        animator.SetBool("Attack" + step, false);
    }

    public void ResetCombo()
    {
        comboStep = 0;
        inputBuffered = false;
        for (int i = 1; i <= 3; i++)
        {
            animator.SetBool("Attack" + i, false);
        }
    }
}
