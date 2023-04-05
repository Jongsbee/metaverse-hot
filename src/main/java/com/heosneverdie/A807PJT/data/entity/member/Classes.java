package com.heosneverdie.A807PJT.data.entity.member;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

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
    @Column(nullable = false, name = "member_id")
    private Long memberId;
    @Column(nullable = false, name = "warrior")
    private boolean isWarriorUnlocked;
    @Column(nullable = false, name = "archer")
    private boolean isArcherUnlocked;
    @Column(nullable = false, name = "hammer")
    private boolean isHammerUnlocked;
    @Column(nullable = false, name = "poor")
    private boolean isPoorUnlocked;
}
