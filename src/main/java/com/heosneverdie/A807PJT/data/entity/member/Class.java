package com.heosneverdie.A807PJT.data.entity.member;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;

@Table(name = "CLASS")
@Entity
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Class {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    // member와 1:1 매핑
    @OneToOne
    @JoinColumn(nullable = false, name = "member_id")
    private Member member;
    @Column(nullable = false, name = "warrior")
    private boolean isWarriorUnlocked;
    @Column(nullable = false, name = "archer")
    private boolean isArcherUnlocked;
}
