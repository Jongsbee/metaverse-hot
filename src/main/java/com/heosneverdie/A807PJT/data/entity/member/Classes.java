package com.heosneverdie.A807PJT.data.entity.member;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.Type;

import javax.persistence.*;

@Table(name = "CLASSES")
@Entity
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Classes {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    // member와 1:1 매핑
    @OneToOne
    @JoinColumn(nullable = false, name = "member_id")
    private Member member;
    @Column(nullable = false, name = "warrior")
    private Boolean isWarriorUnlocked;
    @Column(nullable = false, name = "archer")
    private Boolean isArcherUnlocked;
    @Column(nullable = false, name = "hammer")
    private Boolean isHammerUnlocked;
    @Column(nullable = false, name = "poor")
    private Boolean isPoorUnlocked;

    public void updateHammer() {
        this.isHammerUnlocked = true;
    }
    public void updateDeveloper() {
        this.isPoorUnlocked = true;
    }
}
