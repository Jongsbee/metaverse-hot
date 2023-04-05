package com.heosneverdie.A807PJT.data.entity.member;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;

import javax.persistence.*;

@Table(name = "Account")
@Entity
@Builder
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Account {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    // member와 1:1 매핑
    @Column(nullable = false, name = "member_id")
    private Long memberId;
    @Column(nullable = false)
    private int exp;
    @Column(nullable = false)
    private int coin;
}
